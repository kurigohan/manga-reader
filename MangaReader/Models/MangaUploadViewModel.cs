using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MangaReader.Models
{
    public class MangaUploadViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [DisplayName("Series")]
        public int? SeriesId { get; set; }
        [DisplayName("Collection")]
        public int? CollectionId { get; set; }
        [DisplayName("Artist")]
        public int? ArtistId { get; set; }
        [DisplayName("Language")]
        public int? LanguageId { get; set; }

        public string Tags { get; set; }

        [Required(ErrorMessage = "File required.")]
        [DisplayName("File")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUpload { get; set; }

        public bool Optimize { get; set; }

    }
}
