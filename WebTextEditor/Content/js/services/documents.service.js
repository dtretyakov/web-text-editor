(function() {
    "use strict";

    angular
        .module("services.data", ["ngResource"])
        .factory("documentsService", documentsService);

    documentsService.$inject = ["$resource", "$q"];

    function documentsService($resource, $q) {
        var document = $resource("/api/documents/:id", { id: "@id" }, {
            post: { method: "POST" },
            update: { method: "PUT" }
        });

        return {
            add: add,
            get: get,
            getAll: getAll,
            update: update,
            remove: remove
        };

        function add() {
            var deferred = $q.defer();

            document.post({}, function(data) {
                deferred.resolve(data);
            }, function(error) {
                deferred.reject(error.message);
            });

            return deferred.promise;
        }

        function get(documentId) {
            var deferred = $q.defer();

            document.get({ id: documentId }, function(data) {
                deferred.resolve(data);
            }, function(error) {
                deferred.reject(error.message);
            });

            return deferred.promise;
        }

        function getAll() {
            var deferred = $q.defer();

            document.query(function(data) {
                deferred.resolve(data);
            }, function(error) {
                deferred.reject(error.message);
            });

            return deferred.promise;
        }

        function update(document) {
            var deferred = $q.defer();

            document.update({ id: document.id }, document, function(data) {
                deferred.resolve(data);
            }, function(error) {
                deferred.reject(error.message);
            });

            return deferred.promise;
        }

        function remove(documentId) {
            var deferred = $q.defer();

            document.update({ id: documentId }, function(data) {
                deferred.resolve(data);
            }, function(error) {
                deferred.reject(error.message);
            });

            return deferred.promise;
        }

    }
})();