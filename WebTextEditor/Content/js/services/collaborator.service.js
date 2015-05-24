(function() {
    "use strict";

    angular
        .module("services.collaborator", [])
        .factory("collaboratorService", collaboratorService);

    collaboratorService.$inject = ["generatorFactory"];

    function collaboratorService(generator) {

        var collaborators = {};

        return {
            collaborators: collaborators,
            addOrUpdate: addOrUpdate,
            addList: addList,
            remove: remove,
            updateCarets: updateCarets,
            findAgentId: findAgentId
        };

        /**
         * Adds a collaborator into collection.
         * @param {object} data - collaborator data.
         */
        function addOrUpdate(data) {

            var id = data.connectionId;
            var collaborator = collaborators[id];

            data.color = collaborator
                ? collaborator.color
                : generator.getRandomColor();

            collaborators[id] = data;
        }

        /**
         * Adds a list of collaborators.
         * @param {object} data - collaborators.
         */
        function addList(data) {
            data.forEach(function(collaborator) {
                addOrUpdate(collaborator);
            });
        }

        /**
         * Removes a collaborator from collection.
         * @param {object} data - collaborator data.
         */
        function remove(data) {
            delete collaborators[data.connectionId];
        }

        /**
         * Updates caret positions.
         * @param {object} operation - operation over text.
         */
        function updateCarets(operation) {
            for (var name in collaborators) {
                if (collaborators.hasOwnProperty(name)) {
                    var collaborator = collaborators[name];
                    var caret = collaborator.caretPosition;
                    if (caret == null) continue;

                    collaborator.caretPosition = caret >= operation.end
                        ? caret + operation.start - operation.end + operation.newValue.length
                        : caret;
                }
            }
        }

        /**
         * Finds agent identifier.
         * @param {array} collaborators - collaborators.
         * @param {string} connectionId - connection identifier.
         */
        function findAgentId(connectionId) {
            for (var i = 0; i < collaborators.length; i++) {
                if (collaborators[i].connectionId === connectionId) {
                    return collaborators[i].id;
                }
            }

            return Math.round(Math.random() * 1000);
        }
    }
})();