(function() {
    "use strict";

    angular
        .module("services.input", [])
        .factory("inputElementService", inputElementService);

    function inputElementService() {

        return {
            getSelection: getSelection,
            setSelection: setSelection,
            replaceText: replaceText,
            getChar: getChar
        };

        /**
         * Gets a selection range for the input element.
         * @param {object} element - input element.
         * @return {object} selection range.
         */
        function getSelection(element) {
            var start, end;

            if (element.createTextRange) {
                var r = document.selection.createRange().duplicate();
                r.moveEnd("character", element.value.length);
                if (r.text === "") return element.value.length;
                start = element.value.lastIndexOf(r.text);
                r.moveStart("character", -element.value.length);
                end = r.text.length;
            } else {
                start = element.selectionStart;
                end = element.selectionEnd;
            }

            return { start: start, end: end };
        }

        /**
         * Sets a new selection range.
         * @param {object} element - input element.
         * @param {number} start - start position.
         * @param {number} end - end position.
         */
        function setSelection(element, start, end) {
            end = end || start;

            if (element.createTextRange) {
                var range = element.createTextRange();
                range.collapse(true);
                range.moveStart("character", start);
                range.moveEnd("character", end);
                range.select();
                element.focus();
            } else if (element.setSelectionRange) {
                element.focus();
                element.setSelectionRange(start, end);
            } else if (typeof element.selectionStart != "undefined") {
                element.selectionStart = start;
                element.selectionEnd = end;
                element.focus();
            }
        }

        /**
         * Replaces text at the selected range.
         * @param {object} element - input element.
         * @param {number} start - start position.
         * @param {number} end - end position.
         * @param {string} text - new text.
         */
        function replaceText(element, start, end, text) {
            var selection = getSelection(element);

            if (element.setRangeText) {
                element.setRangeText(text, start, end);
            } else {
                var value = element.value;
                element.value = value.substring(0, start) + value.substring(start + end, value.length);
            }

            // keep relative caret position
            var count = text.length > 0 ? end - start + text.length : start - end;
            selection.start = end <= selection.start ? selection.start + count : selection.start;
            selection.end = end <= selection.end ? selection.end + count : selection.end;
            setSelection(element, selection.start, selection.end);
        }

        /**
         * Returns a character value from the keyboard event.
         * @param {object} event - keyboard event.
         */
        function getChar(event) {
            if (event.ctrlKey ||
                event.keyCode === 8 ||
                event.keyCode === 46 && event.charCode === 0) {
                // Firefix fires keypress events
                return null;
            } else if (event.which == null) {
                // IE
                return String.fromCharCode(event.keyCode);
            } else if (event.which !== 0) {
                // the rest
                return String.fromCharCode(event.which);
            } else {
                // special key
                return null;
            }
        }
    }
})();