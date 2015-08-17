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
            [FromUri] string[] tags = null)
        {

            IEnumerable<Manga> mangaList = db.Manga.ToList();

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
            var mangaDTOList = ConvertToDTO(mangaList);

            /* Sort */
            mangaDTOList = GetOrderedMangaList(mangaDTOList, order, orderBy);


            /* Paginate */
            if (pageSize > 0 && pageNumber > 0)
            {
                mangaDTOList = mangaDTOList
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize);
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
                                (int) Math.Ceiling((double)totalCount / pageSize) : 1;

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
                                    m.Name.Equals(query, StringComparison.InvariantCultureIgnoreCase) ||
                                    (m.Artist != null && m.Artist.Name.Equals(query, StringComparison.InvariantCultureIgnoreCase)) ||
                                    (m.Series != null && m.Series.Name.Equals(query, StringComparison.InvariantCultureIgnoreCase)) ||
                                    (m.Collection != null && m.Collection.Name.Equals(query, StringComparison.InvariantCultureIgnoreCase))
                                )
                                .ToList();

            var matchingTags = db.Tags
                                .Where(t => t.Name.Equals(query, StringComparison.InvariantCultureIgnoreCase))
                                .Select(t => t.Manga)
                                .ToList();

            var mangaDTOList = ConvertToDTO(matchingManga.Union(matchingTags));

            /* Sort */
            mangaDTOList = GetOrderedMangaList(mangaDTOList, order, orderBy);

            /* Get manga tags */
            foreach (var manga in mangaDTOList)
            {
                manga.Tags = db.Tags
                             .Where(t => t.MangaId == manga.Id)
                             .Select(t => t.Name)
                             .ToList();
            }



            var totalCount = mangaDTOList.Count();
            var totalPages = pageSize > 0 && pageNumber > 0 ?
                                (int)Math.Ceiling((double)totalCount / pageSize) : 1;

            /* Return results */

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                MangaList = mangaDTOList
            };

            return Ok(result);
        }




        private IEnumerable<MangaDTO> GetOrderedMangaList(IEnumerable<MangaDTO> mangaList, string order, string orderBy)
        {
            IEnumerable<MangaDTO> orderedMangaList;

            if (orderBy.Length > 0 &&
                typeof(MangaDTO)
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

            if (order.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ||
                order.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                orderedMangaList = mangaList.Reverse();
            }

            return orderedMangaList.ToList();
        }


        private IEnumerable<MangaDTO> ConvertToDTO(IEnumerable<Manga> mangaList)
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
                                });
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