(function () {
    'use strict';
    angular
        .module('mangaReader')
        .provider('AppSettings', AppSettings);

    function AppSettings() {
        var settings = this;
        settings.itemsPerPage = 12;
        settings.defaultOrderBy = 'date';
        settings.defaultOrder = 'desc';

        return {
            $get: function () {
                return {
                    itemsPerPage: settings.itemsPerPage,
                    defaultOrderBy: settings.defaultOrderBy,
                    defaultOrder: settings.defaultOrder
                }
            }
        }
    }


})();