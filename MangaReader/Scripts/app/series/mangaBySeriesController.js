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
            var params = {
                pageSize: vm.itemsPerPage,
                pageNumber: vm.currentPage,
                orderBy: AppSettings.defaultOrderBy,
                order: AppSettings.defaultOrder,
                seriesId: series.id
            };

            MangaService
                .getMangaList(params)
                .then(function (page) {
                    vm.mangaList = page;
                });
        };
    }
})();