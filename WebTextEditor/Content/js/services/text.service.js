(function() {
    "use strict";

    angular
        .module("services.text", ["services.input", "services.collaborator"])
        .factory("textService", textService);

    textService.$inject = [
        "$rootScope",
        "Logoot",
        "LogootText",
        "inputElementService",
        "collaboratorService"
    ];

    function textService($scope, Logoot, LogootText, inputService, collaboratorService) {

        var text = undefined;
        var input = undefined;
        var deferredOps = [];

        return {
            initialize: initialize,
            cut: cut,
            paste: paste,
            keypress: keypress,
            keydown: keydown,
            addCharacters: addCharacters,
            removeCharacters: removeCharacters
        };

        function initialize(atoms, agentId, inputElement) {
            input = inputElement;

            var logoot = new Logoot(atoms);
            text = new LogootText(agentId, logoot);

            deferredOps.forEach(function(op) {
                if (op.type === "ins") {
                    addCharacters(op.values);
                } else if (op.type === "del") {
                    removeCharacters(op.values);
                }
            });

            return text;
        }

        /**
         * Handles cut event.
         * @param {object} event - cut event.
         */
        function cut(event) {
            var selection = inputService.getSelection(event.target);

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
                event.preventDefault();
            }

            // Ctrl + y shortcut
            if (event.ctrlKey && keyCode === 89) {
                event.preventDefault();
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
         * Adds a characters into the text structure.
         * @param {array} values - character values.
         */
        function addCharacters(values) {
            if (!text) {
                deferredOps.push({ type: "ins", values: values });
                return;
            }

            var ops = values.reduce(function (result, value) {
                var op = text.applyOp(["ins", value.id, value.value]);
                var last = result.length > 0 ? result[result.length - 1] : undefined;

                if (last == undefined || op.index !== last.index + last.text.length) {
                    last = { text: "", index: op.index };
                    result.push(last);
                }

                last.text += op.atom;

                return result;
            }, []);

            ops.forEach(function(op) {
                inputService.replaceText(input, op.index, op.index, op.text);
            });

            $scope.$broadcast("elastic:adjust");
        }

        /**
         * Removes a characters from the text structure.
         * @param {array} values - character values.
         */
        function removeCharacters(values) {
            if (!text) {
                deferredOps.push({ type: "del", values: values });
                return;
            }

            var ops = values.reduce(function(result, value) {
                var op = text.applyOp(["del", value.id]);
                var last = result.length > 0 ? result[result.length - 1] : undefined;

                if (last == undefined || op.index !== last.index) {
                    last = { index: op.index, length: 0 };
                    result.push(last);
                }

                last.length++;

                return result;
            }, []);

            ops.forEach(function(op) {
                inputService.replaceText(input, op.index, op.index + op.length, "");
            });

            $scope.$broadcast("elastic:adjust");
        }

        /**
         * Replaces a text in the data structure.
         * @param {number} start - range start.
         * @param {number} end - range end.
         * @param {string} chars - text.
         */
        function replaceText(start, end, chars) {
            var value = LogootText.filterText(chars);

            var operation = {
                start: start,
                end: end,
                newValue: value,
                prevValue: text.str.substring(start, end)
            };

            if (start < end) {
                text.del(start, end);
            }

            if (value.length > 0) {
                text.ins(start, value);
            }

            collaboratorService.updateCarets(operation);
        }
    }
})();