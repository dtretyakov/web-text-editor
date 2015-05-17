(function() {
    "use strict";

    angular
        .module("editor")
        .controller("DocumentController", DocumentController);

    DocumentController.$inject = [
        "$scope",
        "generatorFactory",
        "documentsService",
        "documentsHubService",
        "$routeParams"
    ];

    function DocumentController($scope, generator, documentsService, documentsHubService, $routeParams) {

        var vm = this;

        vm.document = undefined;
        vm.collaborators = {};
        vm.content = {};

        var documentId = $routeParams.documentId;

        activate();

        /// implementation

        function activate() {

            documentsHubService.client.addCollaborator = addOrUpdateCollaborator;
            documentsHubService.client.removeCollaborator = removeCollaborator;
            documentsHubService.client.caretPosition = addOrUpdateCollaborator;
            documentsHubService.connect().then(configureHubConnection);
        }

        function configureHubConnection(connection) {

            connection.joinDocumentEditing(documentId).then(function() {
                documentsService.get(documentId).then(processDocument);
            });

            $scope.$on("$destroy", function() {
                connection.leaveDocumentEditing(documentId);
            });

            $scope.$watch("caretPosition.get", function(value) {
                connection.setCaretPosition(documentId, value);
            });
        }

        function processDocument(document) {

            vm.document = document;
            vm.content = document.content;

            document.collaborators.forEach(function(collab) {
                addOrUpdateCollaborator(collab.userId, collab.caretPosition);
            });
        }

        function addOrUpdateCollaborator(name, caret) {

            var collaborator = vm.collaborators[name] || {
                color: generator.getRandomColor()
            };

            collaborator.caret = caret;

            vm.collaborators[name] = collaborator;
        }

        function removeCollaborator(name) {
            delete vm.collaborators[name];
        }
    }
})();