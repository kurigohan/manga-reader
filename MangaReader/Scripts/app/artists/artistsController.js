(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('ArtistsController', ArtistsController)

    ArtistsController.$inject = [
        '$location',
        '$anchorScroll',
        'artists'];
    function ArtistsController(
        $location,
        $anchorScroll,
        artists) {

        var vm = this;

        // partition artists by first currentLetter
        var artistsByLetter = {};
        var currentLetter, firstLetter, artistList;
        var rowSize = 4;
        var rows = [];
        var count = 0; // length of artistsByLetter
        var letters = []; // list of available letters for nav

        // partition artists by currentLetter and into rows
        angular.forEach(artists, function (a) {
            firstLetter = a.name.charAt(0).toUpperCase();
            if (currentLetter != firstLetter) {
                currentLetter = firstLetter;
                letters.push(currentLetter);
                if (count == rowSize) {
                    rows.push(artistsByLetter);
                    artistsByLetter = {};
                    count = 0;
                }
                artistsByLetter[currentLetter] = [];
                count++;
            }

            artistsByLetter[currentLetter].push(a);
        });
        rows.push(artistsByLetter);

        vm.letters = letters;
        vm.rows = rows;

        vm.goToLetter = function (dest) {
            $location.hash(dest);
            $anchorScroll();
        };
    }
})();
