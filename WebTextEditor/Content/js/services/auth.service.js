(function() {
    "use strict";

    angular
        .module("services.auth", [])
        .factory("authService", authenticationService);

    function authenticationService() {
        var accessToken = "accessToken";

        return {
            getAccessToken: getAccessToken,
            setAccessToken: setAccessToken,
            parseTokenFromUrl: parseTokenFromUrl
        };

        function getAccessToken() {
            return sessionStorage.getItem(accessToken);
        }

        function setAccessToken(value) {
            sessionStorage.setItem(accessToken, value);
        }

        function parseTokenFromUrl(url) {
            var match = url.match(/access_token=([^&]+)/);
            return match && match.length === 2 ? match[1] : undefined;
        }
    }
})();