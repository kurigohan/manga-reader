(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('HomeController', HomeController)

    HomeController.$inject = ['mangaList', 'MangaService', 'AppSettings'];
    function HomeController(mangaList, MangaService, AppSettings) {
        var vm = this;
        vm.currentPage = 1;
        vm.mangaList = mangaList;
        vm.itemsPerPage = AppSettings.itemsPerPage;

        vm.pageChanged = function () {
            var params = { pageSize: vm.itemsPerPage, pageNumber: vm.currentPage };
            MangaService
                .getMangaList(params)
                .then(function (page) {
                    vm.mangaList = page;
                 });
        };
    }
})();