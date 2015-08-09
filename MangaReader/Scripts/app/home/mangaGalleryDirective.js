(function () {
    'use strict';
    angular
        .module('mangaReader')
        .directive('mangaGallery', MangaGallery);

    MangaGallery.$inject = [];
    function MangaGallery() {
        return {
            restrict: 'E',
            scope: {
                data: '='
            },
            templateUrl: 'home/mangaGallery.html'
        }
    };

})();