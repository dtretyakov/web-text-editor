(function() {
    "use strict";

    angular
        .module("directives.inputcaret", [])
        .directive("eInputCaret", inputCaret);

    inputCaret.$inject = ["caretCoordinates"];

    function inputCaret(caretCoordinates) {

        var template = "<div class='cursor'>" +
            "<div class='bar' ng-style='{background: data.color}'>&nbsp;</div>" +
            "<div class='top' ng-style='{background: data.color}'></div>" +
            "<div class='name' ng-style='{background: data.color}'>{{data.name}}</div>" +
            "</div>";

        return {
            restrict: "A",
            replace: true,
            scope: true,
            template: template,
            link: link
        };

        function link(scope, element, attrs) {

            // get input element
            var inputElement = angular.element(attrs.eInputElement);
            if (inputElement.length !== 1) {
                return;
            }

            var input = inputElement[0];


            // watching for caret changes
            scope.$watch(attrs.eInputCaret, function (newValue) {
                if (newValue == undefined) {
                    element.hide(); 
                } else {
                    element.css(caretCoordinates(input, newValue)).show();
                }
            });

            element.hover(function () {
                element.addClass("hover");
            }, function () {
                element.removeClass("hover");
            });
        }
    }

})();