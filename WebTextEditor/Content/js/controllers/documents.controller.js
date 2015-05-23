(function() {
    "use strict";

    angular
        .module("editor")
        .controller("DocumentsController", DocumentsController);

    DocumentsController.$inject = ["documentsService", "$location"];

    function DocumentsController(documentsService, $location) {
        var vm = this;

        vm.documents = [];
        vm.addDocument = addDocument;
        vm.deleteDocument = deleteDocument;
        vm.isLoading = true;

        activate();

        // implementation

        function activate() {
            documentsService.getAll().then(function(data) {
                vm.documents = data;
            }).finally(function() {
                vm.isLoading = false;
            });
        }

        function addDocument() {
            documentsService.add().then(function(document) {
                $location.path("/documents/" + document.id);
            });
        }

        function deleteDocument(document) {
            documentsService.remove(document.id).then(function() {
                var index = vm.documents.indexOf(document);
                if (index > -1) {
                    vm.documents.splice(index, 1);
                }
            });
        }
    }
})();