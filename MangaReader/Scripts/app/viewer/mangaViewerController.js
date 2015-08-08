(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('MangaViewerController', MangaViewerController);

    MangaViewerController.$inject = [
        '$state',
        '$stateParams'
    ];

    function MangaViewerController(
        $state,
        $stateParams
    ) {
        if (!$stateParams.manga) {
            $state.go('home');
        }

        var vm = this;
        vm.manga = $stateParams.manga;
        vm.pagePath = '';
        vm.outerControl = {}; // binds to navControl in pageNavigation directive
    }
})();