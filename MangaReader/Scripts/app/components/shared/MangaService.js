(function () {
    'use strict';
    angular
        .module('mangaReader')
        .factory('MangaService', MangaService);

    MangaService.$inject = ['$http'];
    function MangaService($http) {
        return {
            getManga: getManga,
            getPagePath: getPagePath
        };

        function success(response) {
            return response.data;
        }

        function getManga() {
            return $http
                    .get('/api/manga')
                    .then(success);
        }

        function getPagePath(mangaPath, pageNum) {
            return mangaPath + '/' + pageNum + '.jpg';
        }

    }

})();