using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Models
{

    [MetadataType(typeof(MangaMetaData))]
    public partial class Manga { }
    public class MangaMetaData
    {
        [Display(Name = "Manga")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }


    [MetadataType(typeof(ArtistMetaData))]
    public partial class Artist{}
    public class ArtistMetaData
    {
        [Display(Name = "Artist")]
        public string Name { get; set; }
    }

    [MetadataType(typeof(SeriesMetaData))]
    public partial class Series{}
    public class SeriesMetaData
    {
        [Display(Name = "Series")]
        public string Name { get; set; }
    }

    [MetadataType(typeof(CollectionMetaData))]
    public partial class Collection { }
    public class CollectionMetaData
    {
        [Display(Name = "Collection")]
        public string Name { get; set; }
    }

    [MetadataType(typeof(TagMetaData))]
    public partial class Tag { }
    public class TagMetaData
    {
        [Display(Name = "Tag")]
        public string Name { get; set; }
    }

}
