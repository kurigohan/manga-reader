(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('HomeController', HomeController)

    HomeController.$inject = ['mangaList'];
    function HomeController(mangaList){
        var vm = this;
        vm.mangaList = mangaList;
    }
})();