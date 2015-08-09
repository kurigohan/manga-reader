(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('MangaViewerController', MangaViewerController);

    MangaViewerController.$inject = ['manga', 'pageStart'];

    function MangaViewerController(manga, pageStart) {
        var vm = this;
        vm.manga = manga;
        vm.pagePath = '';
        vm.currentPage = parseInt(pageStart);
        vm.outerControl = {}; // binds to navControl in pageNavigation directive
    }
})();