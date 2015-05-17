(function() {
    "use strict";

    angular
        .module("services.generators", [])
        .factory("generatorFactory", generatorFactory);

    function generatorFactory() {
        return {
            getRandomColor: getRandomColor
        };

        function getRandomColor() {
            var letters = "0123456789ABCDEF".split("");
            var color = "#";
            for (var i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * 16)];
            }
            return color;
        }
    }

})();