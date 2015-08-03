(function () {
    'use strict';
    angular
        .module('mangaReader')
        .directive('pageNavigation', pageNavigation);

    pageNavigation.$inject = ['MangaService'];
    function pageNavigation(MangaService) {
        return {
            restrict: 'E',
            scope: {
                manga: '=',
                control: '=',
                pagePath: '='
            },
            templateUrl: '/Scripts/app/components/viewer/pageNavigation.html',
            link: function (scope) {
                scope.currentPage = 1;
                scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);

                scope.nav = scope.control || {};

                scope.nav.nextPage = function () {
                    if (scope.currentPage < scope.manga.pageCount) {
                        scope.currentPage += 1;
                        scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);
                    }
                };
                
                scope.nav.prevPage = function () {
                    if (scope.currentPage > 1) {
                        scope.currentPage -= 1;
                        scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);
                    }
                };
            }
        }
    };

})();