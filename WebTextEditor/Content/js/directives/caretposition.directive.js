(function() {
    "use strict";

    angular
        .module("directives.caretposition", ["services.input"])
        .directive("eCaret", caretPosition);

    caretPosition.$inject = ["caretPositionService"];

    function caretPosition(caretPositionService) {
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

            element.on("blur", function() {
                element.off("mousemove", updatePosition);
            });

            function updatePosition() {
                scope.$apply(function() {
                    var selection = caretPositionService.getSelection(element[0]);
                    scope.eCaret.start = selection.start;
                    scope.eCaret.end = selection.end;
                });
            }
        }
    }

})();