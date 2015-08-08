(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('MangaDetailsController', MangaDetailsController)

    MangaDetailsController.$inject = ['manga', 'MangaService'];
    function MangaDetailsController(manga, MangaService) {
        var vm = this;
        vm.manga = manga;
        vm.mangaCover = MangaService.getPagePath(manga.path, 1);

        console.log(vm.manga);
    }
})();   