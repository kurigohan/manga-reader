(function () {
    'use strict';
    angular
    	.module('mangaReader')
        .config(routeConfig);
    
    routeConfig.$inject = [
        '$urlRouterProvider',
        '$stateProvider',
        '$uiViewScrollProvider'
    ];

    function routeConfig(
        $urlRouterProvider,
        $stateProvider,
        $uiViewScrollProvider
    ) {
        $uiViewScrollProvider.useAnchorScroll();
        $urlRouterProvider.otherwise('/home');

        $stateProvider
            .state('home', {
                url: '/home',
                templateUrl: 'home/home.html',
                controller: 'HomeController',
                controllerAs: 'Home',
                resolve: {
                    mangaListPage: ['MangaService', 'AppSettings',
                        function (MangaService, AppSettings) {
                            return MangaService
                                    .getMangaListPage(AppSettings.itemsPerPage, 1);
                    }]
                }
            })
            .state('viewer', {
                url: '/viewer/:mangaId/:pageStart?',
                templateUrl: 'viewer/mangaViewer.html',
                controller: 'MangaViewerController',
                controllerAs: 'Viewer',
                params: { manga: null },
                resolve: {
                    manga: ['$stateParams', 'MangaService',
                        function ($stateParams, MangaService) {
                            console.log($stateParams.mangaId);
                            if ($stateParams.manga) {
                                return $stateParams.manga;
                            }
                            return MangaService
                                    .getManga($stateParams.mangaId);
                        }],
                    pageStart: ['$stateParams', function ($stateParams) {
                        return $stateParams.pageStart || 1;
                    }]
                }
            })
            .state('details', {
                url: '/details/:mangaId',
                templateUrl: 'details/mangaDetails.html',
                controller: 'MangaDetailsController',
                controllerAs: 'Details',
                resolve: {
                    manga: ['$stateParams', 'MangaService',
                        function ($stateParams, MangaService) {
                            return MangaService
                                    .getManga($stateParams.mangaId);
                        }]
                }
            });

    }
})();