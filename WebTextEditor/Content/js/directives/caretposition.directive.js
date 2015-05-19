(function() {
    "use strict";

    angular
        .module("directives.caretposition", [])
        .directive("eCaret", caretPosition);

    function caretPosition() {
        return {
            restrict: "A",
            scope: {
                eCaret: "="
            },
            link: link
        };

        function link(scope, element) {
            if (!scope.eCaret) scope.eCaret = {};

            element.on("keydown keyup mousedown mouseup", updatePosition);

            element.on("focus", function() {
                element.on("mousemove", updatePosition);
            });

            element.on("blur", function () {
                element.off("mousemove", updatePosition);
                //scope.$apply(function () {
                //    scope.eCaret.get = undefined;
                //});
            });

            function updatePosition() {
                scope.$apply(function () {
                    scope.eCaret.start = getSelectionStart(element[0]);
                    scope.eCaret.end = getSelectionEnd(element[0]);
                });
            }

            function getSelectionStart(input) {
                if (input.createTextRange) {
                    var r = document.selection.createRange().duplicate();
                    r.moveEnd("character", input.value.length);
                    if (r.text === "") return input.value.length;
                    return input.value.lastIndexOf(r.text);
                } else return input.selectionStart;
            }

            function getSelectionEnd(input) {
                if (input.createTextRange) {
                    var r = document.selection.createRange().duplicate();
                    r.moveStart("character", -input.value.length);
                    return r.text.length;
                } else return input.selectionEnd;
            }
        }
    }

})();