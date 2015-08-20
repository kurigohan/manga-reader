(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('SeriesController', SeriesController)

    SeriesController.$inject = [
        '$location',
        '$anchorScroll',
        'seriesList'];
    function SeriesController(
        $location,
        $anchorScroll,
        seriesList) {

        var vm = this;

        // partition series by first currentLetter
        var seriesByLetter = {};
        var currentLetter, firstLetter, artistList;
        var rowSize = 4;
        var rows = [];
        var count = 0; // length of seriesByLetter
        var letters = []; // list of available letters for nav

        // partition series by currentLetter and into rows
        angular.forEach(seriesList, function (s) {
            firstLetter = s.name.charAt(0).toUpperCase();
            if (currentLetter != firstLetter) {
                currentLetter = firstLetter;
                letters.push(currentLetter);
                if (count == rowSize) {
                    rows.push(seriesByLetter);
                    seriesByLetter = {};
                    count = 0;
                }
                seriesByLetter[currentLetter] = [];
                count++;
            }

            seriesByLetter[currentLetter].push(s);
        });
        rows.push(seriesByLetter);

        vm.letters = letters;
        vm.rows = rows;

        vm.goToLetter = function (dest) {
            $location.hash(dest);
            $anchorScroll();
        };
    }
})();
