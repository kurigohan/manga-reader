(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('MangaByArtistController', MangaByArtistController)

    MangaByArtistController.$inject = ['artist', 'mangaList', 'AppSettings', 'MangaService'];
    function MangaByArtistController(artist, mangaList, AppSettings, MangaService) {
        var vm = this;
        vm.artist = artist;
        vm.currentPage = 1;
        vm.mangaList = mangaList;
        vm.itemsPerPage = AppSettings.itemsPerPage;
        vm.pageChanged = function () {
            MangaService
                .getMangaList(vm.itemsPerPage, vm.currentPage, artist.id)
                .then(function (page) {
                    vm.mangaList = page;
                });
        };
    }
})();