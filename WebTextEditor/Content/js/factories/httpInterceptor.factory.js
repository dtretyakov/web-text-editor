(function() {
    "use strict";

    angular
        .module("services.interceptors", [])
        .factory("httpInterceptor", errorsInterceptor);

    errorsInterceptor.$inject = ["toastr", "$q", "authService"];

    function errorsInterceptor(toastr, $q, authService) {

        return {
            request: request,
            responseError: responseError
        };

        function request(config) {

            config.headers = config.headers || {};
            config.headers.Authorization = "Bearer " + authService.getAccessToken();

            return config;
        }

        function responseError(error) {

            // Show toast notification
            var message = error.status + " " + error.statusText;
            toastr.error(message, null, {
                closeButton: true,
                timeOut: 10000
            });

            return $q.reject(error);
        }
    }

})();