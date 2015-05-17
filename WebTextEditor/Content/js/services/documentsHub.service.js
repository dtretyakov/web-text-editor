(function() {
    "use strict";

    angular
        .module("services.hubs", [])
        .factory("documentsHubService", documentsHubService);

    documentsHubService.$inject = ["$", "$q", "$rootScope"];

    function documentsHubService($, $q, $rootScope) {
        
        var clientFunctions = {};

        return {
            client: clientFunctions,
            connect: connect
        };

        function connect() {

            var deferred = $q.defer();
            var documentsHub = $.connection.documentsHub;

            documentsHub.client.addCollaborator = function() {
                executeFunction(clientFunctions.addCollaborator, arguments);
            };
            documentsHub.client.removeCollaborator = function () {
                executeFunction(clientFunctions.removeCollaborator, arguments);
            };
            documentsHub.client.caretPosition = function () {
                executeFunction(clientFunctions.caretPosition, arguments);
            }

            $.connection.hub.start().done(function () {

                var result = {
                    joinDocumentEditing: documentsHub.server.joinDocumentEditing,
                    leaveDocumentEditing: documentsHub.server.leaveDocumentEditing,
                    setCaretPosition: documentsHub.server.setCaretPosition,
                    addCharacter: documentsHub.server.addCharacter,
                    removeCharacter: documentsHub.server.removeCharacter
                };

                deferred.resolve(result);
            }).fail(function() {
                deferred.reject("Failed to connect to the hub.");
            });

            return deferred.promise;
        }

        function executeFunction(callback, args) {
            if (!callback) {
                return;
            }

            $rootScope.$apply(function () {
                callback.apply(undefined, args);
            });
        }
    }

})();