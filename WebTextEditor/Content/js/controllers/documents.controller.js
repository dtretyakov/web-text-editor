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

        activate();

        // implementation

        function activate() {
            documentsService.getAll().then(function(data) {
                vm.documents = data;
            });
        }

        function addDocument() {
            documentsService.add().then(function (document) {
                $location.path("/documents/" + document.id);
            });
        }
    }
})();