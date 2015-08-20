using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MangaReader.Models;
using System.Reflection;
using MoreLinq;

namespace MangaReader.Controllers.API
{
    [RoutePrefix("api/manga")]
    public class MangaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/manga?pageSize&pageNumber&order&orderBy&artistId&seriesId&collectionId&languageId
        [Route("")]
        public IHttpActionResult GetManga(
            int pageSize = 0,
            int pageNumber = 0,
            string order = "",
            string orderBy = "",
            int? artistId = null,
            int? seriesId = null,
            int? collectionId = null,
            int? languageId = null,
            string artist = "",
            string series = "",
            string collection = "",
            string language = "",
            [FromUri] string[] tags = null)
        {

            IEnumerable<Manga> mangaList = db.Manga.ToList();

            if (!String.IsNullOrEmpty(artist))
            {
                var tempArtist = db.Artists.FirstOrDefault(a => a.Name.Equals(artist, StringComparison.OrdinalIgnoreCase));
                if (tempArtist != null)
                {
                    artistId = tempArtist.Id;
                }
                else
                {
                    return Ok(new { TotalCount = 0, TotalPages = 0, MangaList = new List<MangaDTO>() });
                }

            }

            if (!String.IsNullOrEmpty(series))
            {
                var tempSeries = db.Series.FirstOrDefault(s => s.Name.Equals(series, StringComparison.OrdinalIgnoreCase));
                if (tempSeries != null)
                {
                    seriesId = tempSeries.Id;
                }
                else
                {
                    return Ok(new { TotalCount = 0, TotalPages = 0, MangaList = new List<MangaDTO>() });
                }

            }

            if (!String.IsNullOrEmpty(collection))
            {
                var tempCollection = db.Collections.FirstOrDefault(c => c.Name.Equals(collection, StringComparison.OrdinalIgnoreCase));
                if (tempCollection != null)
                {
                    collectionId = tempCollection.Id;
                }
                else
                {
                    return Ok(new { TotalCount = 0, TotalPages = 0, MangaList = new List<MangaDTO>() });
                }

            }

            if (!String.IsNullOrEmpty(language))
            {
                var tempLanguage = db.Languages.FirstOrDefault(a => a.Name.Equals(language, StringComparison.OrdinalIgnoreCase));
                if (tempLanguage != null)
                {
                    languageId = tempLanguage.Id;
                }
                else
                {
                    return Ok(new { TotalCount = 0, TotalPages = 0, MangaList = new List<MangaDTO>() });
                }
            }

            /* Filter the manga list */
            if (artistId != null)
            {
                mangaList = mangaList
                            .Where(m => m.ArtistId == artistId);
            }

            if (seriesId != null)
            {
                mangaList = mangaList
                            .Where(m => m.SeriesId == seriesId);
            }

            if (collectionId != null)
            {
                mangaList = mangaList
                            .Where(m => m.CollectionId == collectionId);
            }

            if (languageId != null)
            {
                mangaList = mangaList
                             .Where(m => m.LanguageId == languageId);
            }

            /* Convert to MangaDTO list */
            IEnumerable<MangaDTO> mangaDTOList = ConvertToDTO(mangaList);

            /* Sort */
            if (!String.IsNullOrEmpty(orderBy))
            {
                mangaDTOList = GetOrderedMangaList(mangaDTOList, orderBy, order);
            }

            /* Get manga tags */
            foreach (var manga in mangaDTOList)
            {
                manga.Tags = db.Tags
                             .Where(t => t.MangaId == manga.Id)
                             .Select(t => t.Name)
                             .ToList();
            }

            /* Filter based on tags */ 
            if (tags != null && tags.Length > 0)
            {
                mangaDTOList = mangaDTOList
                                .Where(m => m.Tags.Intersect(tags).Count() == tags.Count());
            }


            /* Get pagination info */

            var totalCount = mangaDTOList.Count();
            var totalPages = pageSize > 0 && pageNumber > 0 ?
                                (int)Math.Ceiling((double)totalCount / pageSize) : 1;

            /* Paginate */
            if (pageSize > 0 && pageNumber > 0)
            {
                mangaDTOList = mangaDTOList
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize);
            }



            /* Return results */

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                MangaList = mangaDTOList
            };

            return Ok(result);
        }

        // GET: api/manga/5
        [Route("{id:int}")]
        [ResponseType(typeof(MangaDTO))]
        public IHttpActionResult GetManga(int id)
        {
            Manga m = db.Manga.Find(id);
            if (m == null)
            {
                return NotFound();
            }
            var manga = new MangaDTO
            {
                Id = m.Id,
                Name = m.Name,
                Series = m.Series != null ? m.Series.Name : "",
                Collection = m.Collection != null ? m.Collection.Name : "",
                Artist = m.Artist != null ? m.Artist.Name : "",
                Language = m.Language != null ? m.Language.Name : "",
                PageCount = m.PageCount,
                Path = m.Path,
                Date = m.Date
            };

            manga.Tags = db.Tags
                         .Where(t => t.MangaId == manga.Id)
                         .Select(t => t.Name)
                         .ToList();


            return Ok(manga);
        }

        [Route("search")]
        public IHttpActionResult GetMangaBySearch(
            string query,
            int pageSize = 0,
            int pageNumber = 0,
            string order = "",
            string orderBy = "")
        {


            var matchingManga = db.Manga
                                .Where(m =>
                                    m.Name.Contains(query) ||
                                    (m.Artist != null && m.Artist.Name.Contains(query)) ||
                                    (m.Series != null && m.Series.Name.Contains(query)) ||
                                    (m.Collection != null && m.Collection.Name.Contains(query))
                                )
                                .ToList();

            var matchingTags = db.Tags
                                .Where(t => t.Name.Contains(query))
                                .Select(t => t.Manga)
                                .ToList();

            IEnumerable<MangaDTO> mangaDTOList = ConvertToDTO(matchingManga.Union(matchingTags));

            /* Sort */
            if (!String.IsNullOrEmpty(orderBy))
            {
                mangaDTOList = GetOrderedMangaList(mangaDTOList, orderBy, order);
            }

            /* Get manga tags */
            foreach (var manga in mangaDTOList)
            {
                manga.Tags = db.Tags
                             .Where(t => t.MangaId == manga.Id)
                             .Select(t => t.Name)
                             .ToList();
            }

            /* Get pagination info */
            var totalCount = mangaDTOList.Count();
            var totalPages = pageSize > 0 && pageNumber > 0 ?
                                (int)Math.Ceiling((double)totalCount / pageSize) : 1;

            /* Paginate */
            if (pageSize > 0 && pageNumber > 0)
            {
                mangaDTOList = mangaDTOList
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize);
            }


            /* Return results */

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                MangaList = mangaDTOList
            };

            return Ok(result);
        }




        private IEnumerable<MangaDTO> GetOrderedMangaList(IEnumerable<MangaDTO> mangaList, string orderBy, string order)
        {
            IEnumerable<MangaDTO> orderedMangaList;

            if (typeof(MangaDTO)
                .GetProperty(orderBy,
                    BindingFlags.IgnoreCase
                    | BindingFlags.Public
                    | BindingFlags.Instance) != null)
            {
                orderedMangaList = mangaList
                                    .OrderByDescending(m =>
                                        typeof(MangaDTO)
                                        .GetProperty(orderBy,
                                        BindingFlags.IgnoreCase
                                        | BindingFlags.Public
                                        | BindingFlags.Instance)
                                    .GetValue(m, null));
            }
            else
            {
                orderedMangaList = mangaList.OrderByDescending(m => m.Date);
            }

            if (order.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                order.Equals("ascending", StringComparison.OrdinalIgnoreCase))
            {
                orderedMangaList = orderedMangaList.Reverse();
            }

            return orderedMangaList;
        }


        private List<MangaDTO> ConvertToDTO(IEnumerable<Manga> mangaList)
        {
            return mangaList.Select(m => new MangaDTO
                                {
                                    Id = m.Id,
                                    Name = m.Name,
                                    Series = m.Series != null ? m.Series.Name : "",
                                    Collection = m.Collection != null ? m.Collection.Name : "",
                                    Artist = m.Artist != null ? m.Artist.Name : "",
                                    Language = m.Language != null ? m.Language.Name : "",
                                    PageCount = m.PageCount,
                                    Path = m.Path,
                                    Date = m.Date
                                })
                                .ToList();
        }

        /*
        // PUT: api/Manga/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutManga(int id, Manga manga)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != manga.Id)
            {
                return BadRequest();
            }

            db.Entry(manga).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MangaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Manga
        [ResponseType(typeof(Manga))]
        public IHttpActionResult PostManga(Manga manga)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Manga.Add(manga);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = manga.Id }, manga);
        }

        // DELETE: api/Manga/5
        [ResponseType(typeof(Manga))]
        public IHttpActionResult DeleteManga(int id)
        {
            Manga manga = db.Manga.Find(id);
            if (manga == null)
            {
                return NotFound();
            }

            db.Manga.Remove(manga);
            db.SaveChanges();

            return Ok(manga);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MangaExists(int id)
        {
            return db.Manga.Count(e => e.Id == id) > 0;
        }
    }
}