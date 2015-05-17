(function() {
    "use strict";

    angular
        .module("services.exceptions", [])
        .factory("$exceptionHandler", exceptionHandler);

    exceptionHandler.$inject = ["toastr"];

    function exceptionHandler(toastr) {

        return function(exception) {

            // Show toast notification
            toastr.error(exception.message, null, {
                closeButton: true,
                timeOut: 20000
            });

            // Log in console
            console.log(exception);
        };
    }

})();