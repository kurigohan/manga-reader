(function () {
    'use strict';
    angular
        .module('mangaReader')
        .factory('MangaService', MangaService);

    MangaService.$inject = ['$http', 'Manga'];
    function MangaService($http, Manga) {
        return {
            getMangaList: getMangaList,
            getMangaListPage: getMangaListPage,
            getMangaDetails: getMangaDetails,
            getPagePath: getPagePath
        };

        function success(response) {
            return response.data;
        }

        function getMangaList() {
            return $http
                    .get('/api/manga')
                    .then(success)
                    .then(Manga.fromJson);
        }

        function getMangaListPage(pageSize, pageNum) {
            return $http
                    .get('/api/manga/page/' + pageSize + '/' + pageNum)
                    .then(success)
                    .then(function (data) {
                        data.mangaList = Manga.fromJson(data.mangaList);
                        return data;
                    });
        }

        function getMangaDetails(mangaId) {
            return $http
                    .get('/api/manga/' + mangaId)
                    .then(success)
                    .then(Manga.fromJson);
        }

        function getPagePath(mangaPath, pageNum) {
            return mangaPath + '/' + pageNum + '.jpg';
        }

    }

})();