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
        vm.pages = [];

        for (var i = 1; i < manga.pageCount; i++) {
            vm.pages.push(MangaService.getPagePath(manga.path, i));
        }

        vm.getPagePreview = function (pagePath) {
            return pagePath.slice(0, -4) + 'p.jpg';
        }
    }
})();   