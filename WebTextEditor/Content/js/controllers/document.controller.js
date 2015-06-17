(function() {
    "use strict";

    angular
        .module("editor")
        .controller("DocumentController", DocumentController);

    DocumentController.$inject = [
        "$scope",
        "$routeParams",
        "$location",
        "documentsService",
        "documentsHubService",
        "collaboratorService",
        "textService",
        "ModalService"
    ];

    function DocumentController(
        $scope, $routeParams, $location, documentsService, documentsHubService,
        collaboratorService, textService, modalService) {

        var vm = this;

        vm.document = undefined;
        vm.collaborators = collaboratorService.collaborators;
        vm.connectionId = undefined;
        vm.text = "";
        vm.input = vm.input || {};
        vm.isLoading = true;

        vm.cut = textService.cut;
        vm.paste = textService.paste;
        vm.keypress = textService.keypress;
        vm.keydown = textService.keydown;

        vm.showRenameDialog = showRenameDialog;

        var documentId = $routeParams.documentId;
        var hubConnection = undefined;


        activate();

        /// implementation

        function activate() {

            documentsHubService.client.addCollaborator = collaboratorService.addOrUpdate;
            documentsHubService.client.removeCollaborator = collaboratorService.remove;
            documentsHubService.client.caretPosition = collaboratorService.addOrUpdate;
            documentsHubService.client.addChars = textService.addCharacters;
            documentsHubService.client.removeChars = textService.removeCharacters;
            documentsHubService.client.leaveDocument = leaveDocument;
            documentsHubService.connect().then(configureHubConnection);
        }

        /**
         * Performs configuratrion of hub connection.
         * @param {object} connection - hub connection.
         */
        function configureHubConnection(connection) {

            hubConnection = connection;
            vm.connectionId = connection.connectionId;

            connection.joinDocument(documentId).then(function() {
                documentsService.get(documentId)
                    .then(processDocument)
                    .catch(leaveDocument);

                $scope.$watch("vm.input.start", function(value) {
                    connection.setCaret(documentId, value);
                });
            });

            $scope.$on("$destroy", function() {
                connection.leaveDocument(documentId);
            });
        }

        /**
         * Processes retrieved document.
         * @param {object} document - document data.
         */
        function processDocument(document) {

            vm.document = document;

            collaboratorService.addList(document.collaborators);
            var agentId = collaboratorService.findAgentId(vm.connectionId);

            // Construct CRDT
            var text = textService.initialize(document.content, agentId, vm.input.element);
            text.on("logoot.ops", sendOperations);

            vm.text = text.str;
            vm.isLoading = false;
        }

        /**
         * Sends an operation into downstream.
         * @param {array} ops - operations data.
         */
        function sendOperations(ops) {
            var operation = ops[0];

            if (operation === "ins") {
                hubConnection.addChars(documentId, ops[1]);
            } else if (operation === "del") {
                hubConnection.removeChars(documentId, ops[1]);
            }
        }

        /**
         * Displays a rename document dialog.
         */
        function showRenameDialog() {
            modalService.showModal({
                templateUrl: "editor.rename-document.html",
                controller: "RenameDocumentController",
                controllerAs: "vm",
                inputs: {
                    name: vm.document.name
                }
            }).then(function(modal) {
                modal.element.modal();
                modal.close.then(renameDocument);
            });
        }

        /**
         * Changes the document name.
         * @param {string} name - new document name.
         */
        function renameDocument(name) {
            vm.document.name = name;
            documentsService.update(vm.document);
        }

        /**
         * Changes location to the documents page.
         */
        function leaveDocument() {
            $location.path("/documents");
        }
    }
})();