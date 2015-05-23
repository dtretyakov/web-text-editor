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
            "services.input",
            "services.exceptions",
            "services.generators",
            "services.interceptors",
            "monospaced.elastic",
            "angularModalService"
        ])
        .constant("$", window.$)
        .constant("toastr", window.toastr)
        .constant("caretCoordinates", window.getCaretCoordinates)
        .constant("Logoot", window.Logoot)
        .constant("LogootText", window.LogootText);

})();