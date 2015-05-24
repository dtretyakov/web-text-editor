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
        "$routeParams",
        "Logoot",
        "LogootText",
        "inputElementService",
        "ModalService",
        "$location"
    ];

    function DocumentController(
        $scope, generator, documentsService, documentsHubService, $routeParams,
        Logoot, LogootText, inputService, modalService, $location) {

        var vm = this;

        vm.document = undefined;
        vm.collaborators = {};
        vm.connectionId = undefined;
        vm.text = "";
        vm.input = vm.input || {};
        vm.isLoading = true;

        vm.cut = cut;
        vm.paste = paste;
        vm.keypress = keypress;
        vm.keydown = keydown;

        vm.showRenameDialog = showRenameDialog;

        var documentId = $routeParams.documentId;
        var hubConnection = undefined;
        var text = undefined;
        var undoredoActions = [];
        var lastModification = undefined;

        activate();

        /// implementation

        function activate() {

            documentsHubService.client.addCollaborator = addOrUpdateCollaborator;
            documentsHubService.client.removeCollaborator = removeCollaborator;
            documentsHubService.client.caretPosition = addOrUpdateCollaborator;
            documentsHubService.client.addChar = addCharacterOperation;
            documentsHubService.client.removeChar = removeCharacterOperation;
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
            var agentId = findAgentId(document.collaborators, vm.connectionId);

            // Construct CRDT
            var logoot = new Logoot(document.content);
            logoot.on("ins", insertDocumentCharacter);
            logoot.on("del", removeDocumentCharacter);

            text = new LogootText(agentId, logoot);
            text.on("logoot.op", sendOperation);

            vm.text = text.str;

            document.collaborators.forEach(function(collaborator) {
                addOrUpdateCollaborator(collaborator);
            });

            vm.isLoading = false;
        }

        /**
         * Adds a collaborator into collection.
         * @param {object} data - collaborator data.
         */
        function addOrUpdateCollaborator(data) {

            var id = data.connectionId;
            var collaborator = vm.collaborators[id];

            data.color = collaborator
                ? collaborator.color
                : generator.getRandomColor();

            vm.collaborators[id] = data;
        }

        /**
         * Removes a collaborator from collection.
         * @param {object} data - collaborator data.
         */
        function removeCollaborator(data) {
            delete vm.collaborators[data.connectionId];
        }

        /**
         * Sends an operation into downstream.
         * @param {array} op - operation data.
         */
        function sendOperation(op) {
            var operation = op[0];
            var charId = op[1].join(".");

            if (operation === "ins") {
                hubConnection.addChar(documentId, charId, op[2]);
            } else if (operation === "del") {
                hubConnection.removeChar(documentId, charId);
            }
        }

        /**
         * Handles cut event.
         * @param {object} event - cut event.
         */
        function cut(event) {
            var selection = inputService.getSelection(event.target);

            undoredoActions = [];

            removeText(selection);
        }

        /**
         * Handles paste event.
         * @param {object} event - paste event.
         */
        function paste(event) {
            var selection = inputService.getSelection(event.target);
            var clipboardData = event.originalEvent.clipboardData;
            var value = clipboardData.getData("Text");

            undoredoActions = [];

            insertText(selection, value);
        }

        /**
         * Handles keypress event.
         * @param {object} event - keyboard event.
         */
        function keypress(event) {
            var character = inputService.getChar(event);
            if (!character) return;

            undoredoActions = [];
            lastModification = undefined;

            var selection = inputService.getSelection(event.target);

            deleteText(selection.start, selection.end);

            text.ins(selection.start, character);
        }

        /**
         * Handles keydown event.
         * @param {object} event - keyboard event.
         */
        function keydown(event) {
            var selection = inputService.getSelection(event.target);
            var keyCode = event.keyCode;

            // Backspace key press
            if (keyCode === 8) {

                deleteText(selection.start, selection.end);

                if (selection.start === selection.end && selection.start > 0) {
                    text.del(selection.start - 1);
                }
            }

            // Delete key press
            if (keyCode === 46) {
                var length = text.str.length;

                deleteText(selection.start, selection.end);

                if (selection.start === selection.end && selection.end < length) {
                    text.del(selection.start);
                }
            }

            // Ctrl + z shortcut
            if (event.ctrlKey && keyCode === 90) {
                if (undoredoActions.length !== 0 ||
                    typeof lastModification == "undefined") {

                    event.preventDefault();
                    return;
                }

                undoredoActions.push(lastModification);
                undoredoAction(lastModification);
            }

            // Ctrl + y shortcut
            if (event.ctrlKey && keyCode === 89) {
                if (undoredoActions.length !== 1 ||
                    typeof lastModification == "undefined") {

                    event.preventDefault();
                    return;
                }

                undoredoActions.pop();
                undoredoAction(lastModification);
            }
        }

        /**
         * Deletes text range in between
         * @param {number} start - start position.
         * @param {number} end - end position.
         */
        function deleteText(start, end) {
            for (var len = end - start + 1; --len;) {
                text.del(start);
            }
        }

        /**
         * Undo / redo last action
         * @param {array} operation - last operation.
         */
        function undoredoAction(operation) {
            if (operation[0] === "ins") {
                removeText(operation[1]);
            } else if (operation[0] === "del") {
                insertText(operation[1], operation[2]);
            }
        }

        /**
         * Adds a character into the text structure.
         * @param {string} id - identifier.
         * @param {string} value - character.
         */
        function addCharacterOperation(id, value) {
            if (!text) return;
            text.applyOp(["ins", id, value]);
        }

        /**
         * Removes a character from the text structure.
         * @param {string} id - identifier.
         */
        function removeCharacterOperation(id) {
            if (!text) return;
            text.applyOp(["del", id]);
        }

        /**
         * Finds agent identifier.
         * @param {array} collaborators - collaborators.
         * @param {string} connectionId - connection identifier.
         */
        function findAgentId(collaborators, connectionId) {
            for (var i = 0; i < collaborators.length; i++) {
                if (collaborators[i].connectionId === connectionId) {
                    return collaborators[i].id;
                }
            }

            return Math.round(Math.random() * 1000);
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

        /**
         * Inserts a new text fragment into the document.
         * @param {number} index - position.
         * @param {string} text - text.
         */
        function insertDocumentCharacter(index, text) {
            inputService.replaceText(vm.input.element, index, index, text);
            $scope.$broadcast("elastic:adjust");
        }

        /**
         * Removes document text fragment.
         * @param {number} index - position.
         */
        function removeDocumentCharacter(index) {
            inputService.replaceText(vm.input.element, index, index + 1, "");
            $scope.$broadcast("elastic:adjust");
        }

        /**
         * Inserts text into data structure.
         * @param {object} selection - range.
         * @param {string} chars - text.
         */
        function insertText(selection, chars) {
            var value = LogootText.filterText(chars);
            lastModification = ["ins", { start: selection.start, end: selection.start + value.length }, value];

            deleteText(selection.start, selection.end);

            text.ins(selection.start, value);
        }

        /**
         * Removes text from data structure.
         * @param {object} selection - range.
         */
        function removeText(selection) {
            var value = text.str.substring(selection.start, selection.end);

            lastModification = ["del", { start: selection.start, end: selection.start }, value];

            deleteText(selection.start, selection.end);
        }
    }
})();