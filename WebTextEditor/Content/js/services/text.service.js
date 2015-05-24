(function() {
    "use strict";

    angular
        .module("services.text", ["services.input", "services.collaborator"])
        .factory("textService", textService);

    textService.$inject = [
        "Logoot",
        "LogootText",
        "inputElementService",
        "collaboratorService"
    ];

    function textService(Logoot, LogootText, inputService, collaboratorService) {

        var text = undefined;
        var undoredoActions = [];
        var lastModification = undefined;

        return {
            createLogoot: createLogoot,
            initialize: initialize,
            cut: cut,
            paste: paste,
            keypress: keypress,
            keydown: keydown,
            addCharacter: addCharacter,
            removeCharacter: removeCharacter
        };

        function createLogoot(atoms) {
            return new Logoot(atoms);
        }

        function initialize(agentId, logoot) {
            text = new LogootText(agentId, logoot);
            return text;
        }

        /**
         * Handles cut event.
         * @param {object} event - cut event.
         */
        function cut(event) {
            var selection = inputService.getSelection(event.target);

            undoredoActions = [];

            replaceText(selection.start, selection.end, "");
        }

        /**
         * Handles paste event.
         * @param {object} event - paste event.
         */
        function paste(event) {
            var selection = inputService.getSelection(event.target);
            var clipboardData = event.originalEvent.clipboardData;
            var value = clipboardData.getData("Text");

            undoredoActions = [];

            replaceText(selection.start, selection.end, value);
        }

        /**
         * Handles keypress event.
         * @param {object} event - keyboard event.
         */
        function keypress(event) {
            var character = inputService.getChar(event);
            if (!character) return;

            var selection = inputService.getSelection(event.target);

            replaceText(selection.start, selection.end, character);

            undoredoActions = [];
            lastModification = undefined;
        }

        /**
         * Handles keydown event.
         * @param {object} event - keyboard event.
         */
        function keydown(event) {
            var selection = inputService.getSelection(event.target);
            var keyCode = event.keyCode;

            // Backspace key press
            if (keyCode === 8) {
                backspaceKeyPress(selection);
            }

            // Delete key press
            if (keyCode === 46) {
                deleteKeyPress(selection);
            }

            // Ctrl + z shortcut
            if (event.ctrlKey && keyCode === 90) {
                if (!undoKeyPress()) {
                    event.preventDefault();
                }
            }

            // Ctrl + y shortcut
            if (event.ctrlKey && keyCode === 89) {
                if (!redoKeyPress()) {
                    event.preventDefault();
                }
            }
        }

        /**
         * Handles backspace key press.
         * @param {object} selection - current selected text.
         */
        function backspaceKeyPress(selection) {
            selection.start = selection.start === selection.end && selection.start > 0
                ? selection.start - 1
                : selection.start;

            replaceText(selection.start, selection.end, "");
        }

        /**
         * Handles delete key press.
         * @param {object} selection - current selected text.
         */
        function deleteKeyPress(selection) {
            var length = text.str.length;

            selection.end = selection.start === selection.end && selection.end < length
                ? selection.end + 1
                : selection.end;

            replaceText(selection.start, selection.end, "");
        }

        /**
         * Executes undo operation.
         */
        function undoKeyPress() {
            if (undoredoActions.length !== 0 ||
                typeof lastModification == "undefined") {

                return false;
            }

            undoredoActions.push(lastModification);
            undoredoAction(lastModification);

            return true;
        }

        /**
        * Executes redo operation.
        */
        function redoKeyPress() {
            if (undoredoActions.length !== 1 ||
                typeof lastModification == "undefined") {

                return false;
            }

            undoredoActions.pop();
            undoredoAction(lastModification);

            return true;
        }

        /**
         * Undo / redo last action
         * @param {array} operation - last operation.
         */
        function undoredoAction(operation) {
            var end = operation.prevValue.length > 0
                ? operation.start
                : operation.end + operation.newValue.length;

            replaceText(operation.start, end, operation.prevValue);
        }

        /**
         * Adds a character into the text structure.
         * @param {string} id - identifier.
         * @param {string} value - character.
         */
        function addCharacter(id, value) {
            if (!text) return;
            text.applyOp(["ins", id, value]);
        }

        /**
         * Removes a character from the text structure.
         * @param {string} id - identifier.
         */
        function removeCharacter(id) {
            if (!text) return;
            text.applyOp(["del", id]);
        }

        /**
         * Replaces a text in the data structure.
         * @param {number} start - range start.
         * @param {number} end - range end.
         * @param {string} chars - text.
         */
        function replaceText(start, end, chars) {
            var value = LogootText.filterText(chars);

            lastModification = {
                start: start,
                end: end,
                newValue: value,
                prevValue: text.str.substring(start, end)
            };

            for (var len = end - start + 1; --len;) {
                text.del(start);
            }

            if (value.length > 0) {
                text.ins(start, value);
            }

            collaboratorService.updateCarets(lastModification);
        }
    }
})();