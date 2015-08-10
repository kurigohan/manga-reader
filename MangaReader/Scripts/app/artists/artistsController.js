(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('ArtistsController', ArtistsController)

    ArtistsController.$inject = ['artists'];
    function ArtistsController(artists) {
        var vm = this;
        vm.artists = artists;
    }
})();