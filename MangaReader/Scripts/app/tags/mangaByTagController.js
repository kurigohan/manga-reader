(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('MangaByTagController', MangaByTagController)

    MangaByTagController.$inject = ['tag', 'mangaList', 'AppSettings', 'MangaService'];
    function MangaByTagController(tag, mangaList, AppSettings, MangaService) {
        var vm = this;
        vm.tag = tag;
        vm.currentPage = 1;
        vm.mangaList = mangaList;
        vm.itemsPerPage = AppSettings.itemsPerPage;
        vm.pageChanged = function () {
            var params = {
                pageSize: vm.itemsPerPage,
                pageNumber: vm.currentPage,
                orderBy: AppSettings.defaultOrderBy,
                order: AppSettings.defaultOrder,
                tags: [tag]
            };

            MangaService
                .getMangaList(params)
                .then(function (page) {
                    vm.mangaList = page;
                });
        };
    }
})();