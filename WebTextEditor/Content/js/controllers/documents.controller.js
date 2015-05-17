(function() {
    "use strict";

    angular
        .module("editor")
        .controller("DocumentsController", DocumentsController);

    DocumentsController.$inject = ["documentsService"];

    function DocumentsController(documentsService) {
        var vm = this;

        vm.documents = [];

        activate();

        function activate() {
            documentsService.getAll().then(function(data) {
                vm.documents = data;
            });
        }
    }
})();