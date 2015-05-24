(function() {
    "use strict";

    angular
        .module("editor", [
            "ngAnimate",
            "ngRoute",
            "directives.inputcaret",
            "directives.inputelement",
            "directives.confirm-click",
            "services.auth",
            "services.data",
            "services.hubs",
            "services.input",
            "services.text",
            "services.exceptions",
            "services.generators",
            "services.interceptors",
            "services.collaborator",
            "monospaced.elastic",
            "angularModalService"
        ])
        .constant("$", window.$)
        .constant("toastr", window.toastr)
        .constant("caretCoordinates", window.getCaretCoordinates)
        .constant("Logoot", window.Logoot)
        .constant("LogootText", window.LogootText);

})();