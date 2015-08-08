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
                templateUrl: 'components/home/home.html',
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
                templateUrl: 'components/viewer/mangaViewer.html',
                controller: 'MangaViewerController',
                controllerAs: 'Viewer',
                params: { manga: null }
            });

    }
})();