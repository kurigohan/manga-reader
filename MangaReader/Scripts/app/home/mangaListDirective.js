(function () {
    'use strict';
    angular
        .module('mangaReader')
        .directive('mangaList', MangaList);

    MangaList.$inject = [];
    function MangaList() {
        return {
            restrict: 'E',
            scope: {
                data: '='
            },
            templateUrl: 'home/mangaList.html'
        }
    };

})();