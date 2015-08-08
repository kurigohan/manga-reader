(function () {
    'use strict';
    angular
    	.module('mangaReader')
        .config(routeConfig);
    
    routeConfig.$inject = [
        '$urlRouterProvider',
        '$stateProvider'
    ];

    function routeConfig(
        $urlRouterProvider,
        $stateProvider
    ) {
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
                url: '/viewer',
                templateUrl: 'viewer/mangaViewer.html',
                controller: 'MangaViewerController',
                controllerAs: 'Viewer',
                params: { manga: null }
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
                                    .getMangaDetails($stateParams.mangaId);
                        }]
                }
            });

    }
})();