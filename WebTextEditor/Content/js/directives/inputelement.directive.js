(function() {
    "use strict";

    angular
        .module("directives.inputelement", ["services.input"])
        .directive("eInput", inputElement);

    inputElement.$inject = ["inputElementService"];

    function inputElement(inputService) {
        return {
            restrict: "A",
            scope: {
                eInput: "="
            },
            link: link
        };

        function link(scope, element) {
            if (!scope.eInput) scope.eInput = {};

            var input = element[0];
            scope.eInput.element = input;

            element.on("keydown keyup mousedown mouseup", updatePosition);

            element.on("focus", function() {
                element.on("mousemove", updatePosition);
            });

            element.on("blur", function() {
                element.off("mousemove", updatePosition);
            });

            function updatePosition() {
                scope.$apply(function() {
                    var selection = inputService.getSelection(input);
                    scope.eInput.start = selection.start;
                    scope.eInput.end = selection.end;
                });
            }
        }
    }

})();