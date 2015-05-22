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
        "caretPositionService"
    ];

    function DocumentController(
        $scope, generator, documentsService, documentsHubService,
        $routeParams, Logoot, LogootText, caretService) {

        var vm = this;

        vm.document = undefined;
        vm.collaborators = {};
        vm.connectionId = undefined;
        vm.text = "";
        vm.caret = {};

        vm.cut = cut;
        vm.paste = paste;
        vm.keypress = keypress;
        vm.keydown = keydown;

        var documentId = $routeParams.documentId;
        var hubConnection = undefined;
        var text = undefined;

        activate();

        /// implementation

        function activate() {

            documentsHubService.client.addCollaborator = addOrUpdateCollaborator;
            documentsHubService.client.removeCollaborator = removeCollaborator;
            documentsHubService.client.caretPosition = addOrUpdateCollaborator;
            documentsHubService.client.addChar = addChar;
            documentsHubService.client.removeChar = removeChar;
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
                documentsService.get(documentId).then(processDocument);
            });

            $scope.$on("$destroy", function() {
                connection.leaveDocument(documentId);
            });

            $scope.$watch("vm.caret.start", function(value) {
                connection.setCaret(documentId, value);
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
            text = new LogootText(agentId, logoot);
            text.on("logoot.op", sendOperation);

            vm.text = text.str;

            document.collaborators.forEach(function(collaborator) {
                addOrUpdateCollaborator(collaborator);
            });
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
            var selection = caretService.getSelection(event.target);
            deleteText(selection.start, selection.end);
        }

        /**
         * Handles paste event.
         * @param {object} event - paste event.
         */
        function paste(event) {
            var selection = caretService.getSelection(event.target);
            var clipboardData = event.originalEvent.clipboardData;

            deleteText(selection.start, selection.end);

            var value = clipboardData.getData("Text");
            text.ins(selection.start, value);
        }

        /**
         * Handles keypress event.
         * @param {object} event - keyboard event.
         */
        function keypress(event) {
            var character = getChar(event);
            if (!character || event.ctrlKey) return;

            var selection = caretService.getSelection(event.target);

            deleteText(selection.start, selection.end);

            text.ins(selection.start, character);
        }

        /**
         * Handles keydown event.
         * @param {object} event - keyboard event.
         */
        function keydown(event) {
            var selection = caretService.getSelection(event.target);
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
         * Returns a character value from keyboard event.
         * @param {object} event - keyboard event.
         */
        function getChar(event) {
            if (event.which == null) {
                // IE
                return String.fromCharCode(event.keyCode);
            } else if (event.which !== 0) {
                // the rest
                return String.fromCharCode(event.which);
            } else {
                // special key
                return null;
            }
        }

        /**
         * Adds a character into the document.
         * @param {string} id - identifier.
         * @param {string} value - character.
         */
        function addChar(id, value) {
            text.applyOp(["ins", id, value]);

            vm.text = text.str;
        }

        /**
         * Removes a character from the document.
         * @param {string} id - identifier.
         */
        function removeChar(id) {
            text.applyOp(["del", id]);

            vm.text = text.str;
            vm.caret.start = Math.min(vm.caret.start, vm.text.length);
        }

        /**
         * Finds agent identifier.
         * @param {array} collaborators - collaborators..
         * @param {string} connectionId - connection identifier.
         */
        function findAgentId(collaborators, connectionId) {
            for (var i = 0; i < collaborators.length; i++) {
                if (collaborators[i].connectionId === connectionId) {
                    return collaborators[i].id;
                }
            }

            return connectionId.replace(/-/g, "");
        }
    }
})();