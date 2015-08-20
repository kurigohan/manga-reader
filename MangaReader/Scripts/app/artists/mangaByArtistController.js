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
            var params = {
                pageSize: vm.itemsPerPage,
                pageNumber: vm.currentPage,
                orderBy: AppSettings.defaultOrderBy,
                order: AppSettings.defaultOrder,
                artistId: vm.artist.id
            };

            MangaService
                .getMangaList(params)
                .then(function (page) {
                    vm.mangaList = page;
                });
        };
    }
})();