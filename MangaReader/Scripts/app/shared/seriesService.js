(function () {
    'use strict';
    angular
        .module('mangaReader')
        .factory('SeriesService', SeriesService);

    SeriesService.$inject = ['$http'];
    function SeriesService($http) {
        return {
            getSeries: getSeries,
            getSeriesList: getSeriesList
        };

        function success(response) {
            return response.data;
        }

        function getSeriesList(params) {
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
                    .get('/api/series' + query)
                    .then(success);
        }

        function getSeries(seriesId) {
            return $http
                    .get('/api/series/' + seriesId)
                    .then(success);
        }
    }

})();