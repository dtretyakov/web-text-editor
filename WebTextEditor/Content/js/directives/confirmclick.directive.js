(function() {
    "use strict";

    angular
        .module("directives.confirm-click", [])
        .directive("eConfirmClick", confirmClick);

    function confirmClick() {
        return {
            restrict: "A",
            link: function(scope, elt, attrs) {
                elt.bind("click", function() {
                    var message = attrs.eConfirmation || "Are you sure?";
                    if (window.confirm(message)) {
                        var action = attrs.eConfirmClick;
                        if (action)
                            scope.$apply(scope.$eval(action));
                    }
                });
            }
        };
    }
})();