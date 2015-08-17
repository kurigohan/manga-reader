(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('SearchController', SearchController)

    SearchController.$inject = [
        'mangaList',
        'MangaService',
        'AppSettings',
        'query'
    ];
    function SearchController(
        mangaList,
        MangaService,
        AppSettings,
        query
    ) {
        var vm = this;
        vm.currentPage = 1;
        vm.mangaList = mangaList;
        vm.itemsPerPage = AppSettings.itemsPerPage;
        vm.query = query;

        vm.pageChanged = function () {
            var params = {
                pageSize: vm.itemsPerPage,
                pageNumber: vm.currentPage,
                query: vm.query
            };
            MangaService
                .getMangaBySearch(params)
                .then(function (page) {
                    vm.mangaList = page;
                 });
        };
    }
})();