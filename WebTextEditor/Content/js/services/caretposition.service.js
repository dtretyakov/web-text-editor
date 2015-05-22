(function() {
    "use strict";

    angular
        .module("services.input", [])
        .factory("caretPositionService", caretPositionService);

    function caretPositionService() {

        return {
            getSelection: getSelection
        };

        function getSelection(input) {
            var start, end;

            if (input.createTextRange) {
                var r = document.selection.createRange().duplicate();
                r.moveEnd("character", input.value.length);
                if (r.text === "") return input.value.length;
                start = input.value.lastIndexOf(r.text);
                r.moveStart("character", -input.value.length);
                end = r.text.length;
            } else {
                start = input.selectionStart;
                end = input.selectionEnd;
            }

            return { start: start, end: end };
        }
    }
})();