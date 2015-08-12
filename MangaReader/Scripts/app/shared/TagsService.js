(function () {
    'use strict';
    angular
        .module('mangaReader')
        .factory('TagsService', TagsService);

    TagsService.$inject = ['$http'];
    function TagsService($http) {
        return {
            getTags: getTags
        };

        function success(response) {
            return response.data;
        }

        function getTags(params) {
            var query = '';
            if (params) {
                query = '?';
                var queryParams = '';
                if (params.order) {
                    queryParams += '&order=' + params.order;
                }
                query += queryParams.substring(1);
            }

            return $http
                    .get('/api/tags' + query)
                    .then(success);
        }
    }

})();