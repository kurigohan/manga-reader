﻿(function () {
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
            MangaService
                .getMangaList(vm.itemsPerPage, vm.currentPage)
                .then(function (page) {
                    vm.mangaList = page;
                 });
        };
    }
})();