(function () {
    'use strict';
    angular
        .module('mangaReader')
        .provider('AppSettings', AppSettings);

    function AppSettings() {
        var itemsPerPage = 12;

        return {
            setItemsPerPage: setItemsPerPage,
            $get: function () {
                return {
                    itemsPerPage: itemsPerPage
                }
            }
        }

        function setItemsPerPage(value) {
            itemsPerPage = value;
        }
    }


})();