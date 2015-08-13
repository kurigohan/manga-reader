(function () {
    'use strict';
    angular
        .module('mangaReader')
        .factory('MangaService', MangaService);

    MangaService.$inject = ['$http', 'Manga'];
    function MangaService($http, Manga) {
        return {
            getManga: getManga,
            getAllManga: getAllManga,
            getMangaList: getMangaList,
            getPagePath: getPagePath
        };

        function success(response) {
            return response.data;
        }

        function getManga(mangaId) {
            return $http
                    .get('/api/manga/' + mangaId)
                    .then(success)
                    .then(Manga.fromJson);
        }

        function getAllManga() {
            return $http
                    .get('/api/manga')
                    .then(success)
                    .then(Manga.fromJson);
        }

        function getMangaList(params) {
            var query = '';

            if (params) {
                query = '?';
                var queryParams = '';

                if (params.pageSize) {
                    queryParams += '&pageSize=' + params.pageSize;
                }
                if (params.pageNumber) {
                    queryParams += '&pageNumber=' + params.pageNumber;
                }
                if (params.artistId) {
                    queryParams += '&artistId=' + params.artistId;
                }

                if (params.seriesId) {
                    queryParams += '&seriesId=' + params.seriesId;
                }

                if (params.collectionId) {
                    queryParams += '&collectionId=' + params.collectionId;
                }

                if (params.languageId) {
                    queryParams += '&languageId=' + params.languageId;
                }

                if (params.tags) {
                    angular.forEach(params.tags, function (t) {
                        queryParams += '&tags=' + t;
                    });
                }

                query += queryParams.substring(1);
            }

            return $http
                    .get('/api/manga' + query)
                    .then(success)
                    .then(function (data) {
                        data.mangaList = Manga.fromJson(data.mangaList);
                        return data;
                    });
        }

        function getPagePath(mangaPath, pageNum) {
            return mangaPath + '/' + pageNum + '.jpg';
        }
    }

})();