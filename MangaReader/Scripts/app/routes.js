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
                    mangaList: ['MangaService', 'AppSettings',
                        function (MangaService, AppSettings) {
                            var params = {
                                pageSize: AppSettings.itemsPerPage,
                                pageNumber: 1
                            };
                            return MangaService
                                    .getMangaList(params);
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
            })
            .state('artists', {
                url: '/artists',
                templateUrl: 'artists/artists.html',
                controller: 'ArtistsController',
                controllerAs: 'Artists',
                resolve: {
                    artists: ['ArtistsService', function (ArtistsService) {
                        return ArtistsService.getArtists({ orderBy: 'name', order: 'asc' });
                    }]
                }
            })
            .state('mangaByArtist', {
                url: '/mangaByArtist/:artistId',
                templateUrl: 'artists/mangaByArtist.html',
                controller: 'MangaByArtistController',
                controllerAs: 'MangaByArtist',
                params: { artist: null },
                resolve: {
                    artist: ['$stateParams', 'ArtistsService',
                        function ($stateParams, ArtistsService) {
                            if ($stateParams.artist) {
                                return $stateParams.artist;
                            }
                            return ArtistsService
                                    .getArtist($stateParams.artistId);
                        }],
                    mangaList: ['$stateParams', 'MangaService', 'AppSettings',
                        function ($stateParams, MangaService, AppSettings) {
                            var artistId = $stateParams.artistId || $stateParams.artist.id;
                            var params = {
                                pageSize: AppSettings.itemsPerPage,
                                pageNumber: 1,
                                artistId: artistId
                            };
                            return MangaService
                                    .getMangaList(params);
                        }]
                }
            })
            .state('series', {
                url: '/series',
                templateUrl: 'series/series.html',
                controller: 'SeriesController',
                controllerAs: 'Series',
                resolve: {
                    seriesList: ['SeriesService', function (SeriesService) {
                        return SeriesService.getSeriesList({ orderBy: 'name', order: 'asc' });
                    }]
                }
            })
            .state('mangaBySeries', {
                url: '/mangaBySeries/:seriesId',
                templateUrl: 'series/mangaBySeries.html',
                controller: 'MangaBySeriesController',
                controllerAs: 'MangaBySeries',
                params: { series: null },
                resolve: {
                    series: ['$stateParams', 'SeriesService',
                        function ($stateParams, SeriesService) {
                            if ($stateParams.series) {
                                return $stateParams.series;
                            }
                            return SeriesService
                                    .getSeries($stateParams.seriesId);
                        }],
                    mangaList: ['$stateParams', 'MangaService', 'AppSettings',
                        function ($stateParams, MangaService, AppSettings) {
                            var seriesId = $stateParams.seriesId || $stateParams.series.id;
                            var params = {
                                pageSize: AppSettings.itemsPerPage,
                                pageNumber: 1,
                                seriesId: seriesId
                            };
                            return MangaService
                                    .getMangaList(params);
                        }]
                }
            })
            .state('tags', {
                url: '/tags',
                templateUrl: 'tags/tags.html',
                controller: 'TagsController',
                controllerAs: 'Tags',
                resolve: {
                    tags: ['TagsService', function (TagsService) {
                        return TagsService.getTags({ order: 'asc' });
                    }]
                }
            });



    }
})();