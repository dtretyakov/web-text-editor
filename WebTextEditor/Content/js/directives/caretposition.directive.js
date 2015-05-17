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

            element.on("keydown keyup click focus", function () {
                scope.$apply(function () {
                    scope.eCaret.get = getPos(element[0]);
                });
            });

            element.on("blur", function () {
                //scope.$apply(function () {
                //    scope.eCaret.get = undefined;
                //});
            });

            scope.$watch("eCaret.set", function (newVal) {
                if (typeof newVal === "undefined") return;
                setPos(element[0], newVal);
            });
        }

        function getPos(element) {
            if ("selectionStart" in element) {
                return element.selectionStart;
            } else if (document.selection) {
                element.focus();
                var sel = document.selection.createRange();
                var selLen = document.selection.createRange().text.length;
                sel.moveStart("character", -element.value.length);
                return sel.text.length - selLen;
            }
            return undefined;
        }

        function setPos(element, caretPos) {
            if (element.createTextRange) {
                var range = element.createTextRange();
                range.move("character", caretPos);
                range.select();
            } else {
                element.focus();
                if (element.selectionStart !== undefined) {
                    element.setSelectionRange(caretPos, caretPos);
                }
            }
        }
    }

})();