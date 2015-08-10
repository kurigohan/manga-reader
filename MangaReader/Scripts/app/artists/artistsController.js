(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('ArtistsController', ArtistsController)

    ArtistsController.$inject = ['artists'];
    function ArtistsController(artists) {
        var vm = this;
        vm.artists = artists;

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
    }
})();
