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
            documentsHub.client.addChar = function () {
                executeFunction(clientFunctions.addChar, arguments);
            };
            documentsHub.client.removeChar = function () {
                executeFunction(clientFunctions.removeChar, arguments);
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
                    addChar: documentsHub.server.addChar,
                    removeChar: documentsHub.server.removeChar
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