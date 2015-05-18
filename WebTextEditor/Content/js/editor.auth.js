(function() {
    "use strict";

    angular
        .module("editor")
        .run(configureAuth);

    configureAuth.$inject = ["authService", "$location", "$window"];

    function configureAuth(authService, $location, $window) {
        // checks whether access token present
        if (!authService.getAccessToken()) {
            var params = authService.parseTokenFromUrl($location.path());
            if (params.access_token) {
                // returning with access token, restore old hash, or at least hide token
                $location.path(params.state || "/");
                authService.setAccessToken(params.access_token);
            } else {
                // no token - so bounce to Authorize endpoint in AccountController to sign in or register
                $window.location.href = "/account/authorize?client_id=web&response_type=token&state=" + encodeURIComponent($location.path());
            }
        }
    }

})();