(function () {
    'use strict';
    angular
        .module('mangaReader')
        .directive('searchBar', searchBarDirective);

    searchBarDirective.$inject = ['$state'];
    function searchBarDirective($state){
        return {
            restrict: 'E',
            templateUrl: 'search/searchBar.html',
            scope: {},
            link: function (scope) {
                scope.query = '';
                scope.search = function () {
                    $state.go('search', {query: scope.query});
                };
            }
        };
    }
})();