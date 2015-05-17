(function() {
    "use strict";

    angular
        .module("editor", [
            "ngAnimate",
            "ngRoute",
            "directives.caretposition",
            "directives.inputcaret",
            "services.auth",
            "services.data",
            "services.hubs",
            "services.exceptions",
            "services.generators",
            "services.interceptors"
        ])
        .constant("$", window.$)
        .constant("toastr", window.toastr)
        .constant("caretCoordinates", window.getCaretCoordinates);

})();