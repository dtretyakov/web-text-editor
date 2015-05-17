(function() {
    "use strict";

    angular
        .module("editor")
        .config(configureLocation);

    configureLocation.$inject = ["$routeProvider"];

    function configureLocation($routeProvider) {

        $routeProvider.
            when("/", {
                templateUrl: "editor.home.html",
                controller: "HomeController",
                controllerAs: "vm"
            }).
            when("/documents", {
                templateUrl: "editor.documents.html",
                controller: "DocumentsController",
                controllerAs: "vm"
            }).
            when("/documents/:documentId", {
                templateUrl: "editor.document.html",
                controller: "DocumentController",
                controllerAs: "vm"
            });
    }

})();