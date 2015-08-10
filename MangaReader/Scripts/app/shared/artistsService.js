(function () {
    'use strict';
    angular
        .module('mangaReader')
        .factory('ArtistsService', ArtistsService);

    ArtistsService.$inject = ['$http'];
    function ArtistsService($http) {
        return {
            getArtists: getArtists,
            getArtist: getArtist
        };

        function success(response) {
            return response.data;
        }

        function getArtists() {
            return $http
                    .get('/api/artists')
                    .then(success);
        }

        function getArtist(artistId) {
            return $http
                    .get('/api/artists/' + artistId)
                    .then(success);
        }
    }

})();