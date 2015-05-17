(function() {
    "use strict";

    angular
        .module("editor")
        .run(configureAuth);

    configureAuth.$inject = ["authService", "$location", "$window"];

    function configureAuth(authService, $location, $window) {
        // checks whether access token present
        if (!authService.getAccessToken()) {
            var accessToken = authService.parseTokenFromUrl($location.path());
            if (accessToken) {
                // returning with access token, restore old hash, or at least hide token
                $location.path("/");
                authService.setAccessToken(accessToken);
            } else {
                // no token - so bounce to Authorize endpoint in AccountController to sign in or register
                $window.location.href = "/account/authorize?client_id=web&response_type=token&state=" + encodeURIComponent("#" + $location.path());
            }
        }
    }

})();