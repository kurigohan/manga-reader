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
            templateUrl: '/Scripts/app/components/shared/mangaList.html'
        }
    };

})();