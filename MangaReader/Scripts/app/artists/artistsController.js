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
        artists)
    {
        var vm = this;
        vm.artists = artists;

        // partition artists by first letter
        var artistsByLetter = {};
        var letter, firstLetter, artistList;
        angular.forEach(artists, function (a) {
            firstLetter = a.name.charAt(0).toUpperCase();
            if (letter != firstLetter) {
                letter = firstLetter;
                artistsByLetter[letter] = [];
            }
            artistsByLetter[letter].push(a);
        });

        vm.artistsByLetter = artistsByLetter;

        vm.goToLetter = function (dest) {
            $location.hash(dest);
            $anchorScroll();
        };
    }
})();
