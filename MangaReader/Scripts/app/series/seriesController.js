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

        // partition seriesList by first letter
        var seriesByLetter = {};
        var letter, firstLetter, artistList;
        angular.forEach(seriesList, function (s) {
            firstLetter = s.name.charAt(0).toUpperCase();
            if (letter != firstLetter) {
                letter = firstLetter;
                seriesByLetter[letter] = [];
            }
            seriesByLetter[letter].push(s);
        });

        vm.seriesByLetter = seriesByLetter;

        vm.goToLetter = function (dest) {
            $location.hash(dest);
            $anchorScroll();
        };
    }
})();
