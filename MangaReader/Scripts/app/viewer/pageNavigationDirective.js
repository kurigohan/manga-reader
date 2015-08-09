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
                pagePath: '=',
                currentPage: '='
            },
            templateUrl: 'viewer/pageNavigation.html',
            link: function (scope) {
                if (!scope.currentPage) {
                    scope.currentPage = 1;
                }

                scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);

                scope.nav = scope.control || {};

                scope.nav.nextPage = function () {
                    if (scope.currentPage < scope.manga.pageCount) {
                        scope.currentPage += 1;
                        scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);
                        window.scrollTo(0, 0);
                    }
                };
                
                scope.nav.prevPage = function () {
                    if (scope.currentPage > 1) {
                        scope.currentPage -= 1;
                        scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);
                        window.scrollTo(0, 0);
                    }
                };

                scope.nav.firstPage = function () {
                    if (scope.currentPage > 1) {
                        scope.currentPage = 1;
                        scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);
                        window.scrollTo(0, 0);
                    }
                };

                scope.nav.lastPage = function () {
                    if (scope.currentPage < scope.manga.pageCount) {
                        scope.currentPage = scope.manga.pageCount;
                        scope.pagePath = MangaService.getPagePath(scope.manga.path, scope.currentPage);
                        window.scrollTo(0, 0);
                    }
                }


            }
        }
    };

})();