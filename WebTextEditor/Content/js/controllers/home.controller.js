(function() {
    "use strict";

    angular
        .module("editor")
        .controller("HomeController", HomeController);

    HomeController.$inject = ["documentsService", "$location"];

    function HomeController(documentsService, $location) {
        var vm = this;

        vm.addDocument = addDocument;
        vm.isBusy = false;

        function addDocument() {
            vm.isBusy = true;

            documentsService.add().then(function(document) {
                $location.path("/documents/" + document.id);
            }).finally(function() {
                vm.isBusy = false;
            });
        }
    }
})();