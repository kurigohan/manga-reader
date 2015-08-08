(function () {
    'use strict';
    angular
        .module("mangaReader")
        .factory("Manga", MangaModel);

    function MangaModel() {
        function Manga(id, name, artist, series, collection, language, date, pageCount, path) {
            this.id = id;
            this.name = name;
            this.artist = artist;
            this.series = series;
            this.collection = collection;
            this.language = language;
            this.date = date;
            this.pageCount = pageCount;
            this.path = path;
        }

        /**
         * Public methods, assigned to prototype
         */

        ////////////////////////////////

        /**
         * Static methods, assigned to class
         * Instance ('this') is not available in static context
         */
        Manga.build = function (data) {
            return new Manga(
                data.id,
                data.name,
                data.artist,
                data.series,
                data.collection,
                data.language,
                data.date,
                data.pageCount,
                data.path
                );
        }

        Manga.fromJson = function (json) {
            if (angular.isArray(json)) {
                return json.map(Manga.build);
            }
            return Manga.build(json);
        };

        return Manga;
    }
})();