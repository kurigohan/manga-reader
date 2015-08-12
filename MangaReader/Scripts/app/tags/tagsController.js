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

        // partition tags by first letter
        var tagsByLetter = {};
        var letter, firstLetter, tagList;
        angular.forEach(tags, function (t) {
            firstLetter = t.charAt(0).toUpperCase();
            if (letter != firstLetter) {
                letter = firstLetter;
                tagsByLetter[letter] = [];
            }
            tagsByLetter[letter].push(t);
        });

        vm.tagsByLetter = tagsByLetter;

        vm.goToLetter = function (dest) {
            $location.hash(dest);
            $anchorScroll();
        };
    }
})();
