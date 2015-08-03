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
                templateUrl: '/Scripts/app/components/home/home.html',
                controller: 'HomeController',
                controllerAs: 'Home',
                resolve: {
                    mangaList: ['MangaService', function (MangaService) {
                        return MangaService.getManga();
                    }]
                }
            })
            .state('viewer', {
                url: '/viewer',
                templateUrl: '/Scripts/app/components/viewer/mangaViewer.html',
                controller: 'MangaViewerController',
                controllerAs: 'Viewer',
                params: { manga: null }
            });

    }
})();