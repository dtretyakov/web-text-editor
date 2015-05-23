(function() {
    "use strict";

    angular
        .module("editor")
        .controller("RenameDocumentController", RenameDocumentController);

    RenameDocumentController.$inject = ["$scope", "name", "close"];

    function RenameDocumentController($scope, name, close) {

        var vm = this;

        vm.name = name;
        vm.saveChanges = saveChanges;

        // implementation

        /**
         * Saves a document name.
         */
        function saveChanges() {
            close(vm.name, 200);
        }
    }
})();