using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MangaReader.Models
{
    public class MangaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Collection { get; set; }
        public string Artist { get; set; }
        public string Language { get; set; }
        public int PageCount { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; }
    }
}
