using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MangaReader.Models
{

    public class Manga
    {

        public int Id { get; set; }

        [DisplayName("Manga")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int? SeriesId { get; set; }
        public virtual Series Series { get; set; }

        public int? CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        public int? ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        public int? LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public int PageCount { get; set; }

        public string Path { get; set; }
    }

    public class Series
    {
        public int Id { get; set; }
        [DisplayName("Series")]
        public string Name { get; set; }
    }

    public class Collection
    {
        public int Id { get; set; }
        [DisplayName("Collection")]
        public string Name { get; set; }
    }

    public class Artist
    {
        public int Id { get; set; }
        [DisplayName("Artist")]
        public string Name { get; set; }
    }

    public class Language
    {
        public int Id { get; set; }
        [DisplayName("Language")]
        public string Name { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        [DisplayName("Tag")]
        public string Name { get; set; }
        public int MangaId { get; set; }
        public virtual Manga Manga { get; set; }
    }
}