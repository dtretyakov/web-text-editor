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
            var match,
                pl = /\+/g, // Regex for replacing addition symbol with a space
                search = /([^\/&=]+)=?([^&]*)/g,
                decode = function(s) { return decodeURIComponent(s.replace(pl, " ")); };

            var urlParams = {};
            while ((match = search.exec(url)))
                urlParams[decode(match[1])] = decode(match[2]);

            return urlParams;
        }
    }
})();