﻿(function () {
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

        function getArtists(params) {
            var query = '';
            if (params) {
                query = '?';
                var queryParams = '';

                if (params.orderBy) {
                    queryParams += '&orderBy=' + params.orderBy;
                }
                if (params.order) {
                    queryParams += '&order=' + params.order;
                }
                query += queryParams.substring(1);
            }

            return $http
                    .get('/api/artists' + query)
                    .then(success);
        }

        function getArtist(artistId) {
            return $http
                    .get('/api/artists/' + artistId)
                    .then(success);
        }
    }

})();