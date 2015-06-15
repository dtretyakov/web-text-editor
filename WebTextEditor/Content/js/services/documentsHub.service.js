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
            var documentsHub = $.connection.docs;

            documentsHub.client.addCollaborator = function() {
                executeFunction(clientFunctions.addCollaborator, arguments);
            };
            documentsHub.client.removeCollaborator = function () {
                executeFunction(clientFunctions.removeCollaborator, arguments);
            };
            documentsHub.client.caretPosition = function () {
                executeFunction(clientFunctions.caretPosition, arguments);
            }
            documentsHub.client.addChars = function () {
                executeFunction(clientFunctions.addChars, arguments);
            };
            documentsHub.client.removeChars = function () {
                executeFunction(clientFunctions.removeChars, arguments);
            };
            documentsHub.client.leaveDocument = function () {
                executeFunction(clientFunctions.leaveDocument, arguments);
            };

            $.connection.hub.start().done(function (connection) {

                var result = {
                    connectionId: connection.id,
                    joinDocument: documentsHub.server.joinDocument,
                    leaveDocument: documentsHub.server.leaveDocument,
                    setCaret: documentsHub.server.setCaret,
                    addChars: documentsHub.server.addChars,
                    removeChars: documentsHub.server.removeChars
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