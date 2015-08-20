(function () {
    'use strict';
    angular
        .module('mangaReader')
        .controller('TagsController', TagsController)

    TagsController.$inject = [
        '$location',
        '$anchorScroll',
        'tags'];
    function TagsController(
        $location,
        $anchorScroll,
        tags) {

        var vm = this;

        // partition tags by first currentLetter
        var tagsByLetter = {};
        var currentLetter, firstLetter, artistList;
        var rowSize = 4;
        var rows = [];
        var count = 0; // length of tagsByLetter
        var letters = []; // list of available letters for nav

        // partition tags by currentLetter and into rows
        angular.forEach(tags, function (t) {
            firstLetter = t.charAt(0).toUpperCase();
            if (currentLetter != firstLetter) {
                currentLetter = firstLetter;
                letters.push(currentLetter);
                if (count == rowSize) {
                    rows.push(tagsByLetter);
                    tagsByLetter = {};
                    count = 0;
                }
                tagsByLetter[currentLetter] = [];
                count++;
            }

            tagsByLetter[currentLetter].push(t);
        });
        rows.push(tagsByLetter);

        vm.letters = letters;
        vm.rows = rows;

        vm.goToLetter = function (dest) {
            $location.hash(dest);
            $anchorScroll();
        };
    }
})();
