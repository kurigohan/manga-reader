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

        function getMangaList(pageSize, pageNum, artistId, seriesId, collectionId, languageId) {
            var query = '?pageSize=' + pageSize + '&pageNumber=' + pageNum;

            if (artistId) {
                query += '&artistId=' + artistId;
            }

            if (seriesId) {
                query += '&seriesId=' + seriesId;
            }

            if (collectionId) {
                query += '&collectionId=' + collectionId;
            }

            if (languageId) {
                query += '&languageId=' + languageId;
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