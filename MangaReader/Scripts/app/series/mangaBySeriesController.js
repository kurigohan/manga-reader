(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('MangaBySeriesController', MangaBySeriesController)

    MangaBySeriesController.$inject = ['series', 'mangaList', 'AppSettings', 'MangaService'];
    function MangaBySeriesController(series, mangaList, AppSettings, MangaService) {
        var vm = this;
        vm.series = series;
        vm.currentPage = 1;
        vm.mangaList = mangaList;
        vm.itemsPerPage = AppSettings.itemsPerPage;
        vm.pageChanged = function () {
            MangaService
                .getMangaList(vm.itemsPerPage, vm.currentPage, series.id)
                .then(function (page) {
                    vm.mangaList = page;
                });
        };
    }
})();