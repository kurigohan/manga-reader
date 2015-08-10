(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('HomeController', HomeController)

    HomeController.$inject = ['mangaListPage', 'MangaService', 'AppSettings'];
    function HomeController(mangaListPage, MangaService, AppSettings) {
        var vm = this;
        vm.currentPage = 1;
        vm.mangaListPage = mangaListPage;
        vm.itemsPerPage = AppSettings.itemsPerPage;

        vm.pageChanged = function () {
            MangaService
                .getMangaListPage(vm.itemsPerPage, vm.currentPage)
                .then(function (page) {
                    vm.mangaListPage = page;
                 });
        };
    }
})();