(function() {
    "use strict";

    angular
        .module("editor")
        .config(httpProviderConfig)
        .config(locationConfig);

    // configure http provider

    httpProviderConfig.$inject = ["$httpProvider"];

    function httpProviderConfig($httpProvider) {
        $httpProvider.interceptors.push("httpInterceptor");
    }

    // configure location provider

    locationConfig.$inject = ["$locationProvider"];

    function locationConfig($locationProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }

})();