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
            int? languageId = null)
        {

            var mangaList = db.Manga.ToList();

            /* Filter the manga list */
            if (artistId != null)
            {
                mangaList = mangaList
                            .Where(m => m.ArtistId == artistId)
                            .ToList();
            }

            if (seriesId != null)
            {
                mangaList = mangaList
                            .Where(m => m.SeriesId == seriesId)
                            .ToList();
            }

            if (collectionId != null)
            {
                mangaList = mangaList
                            .Where(m => m.CollectionId == collectionId)
                            .ToList();
            }

            if (languageId != null)
            {
                mangaList = mangaList
                             .Where(m => m.LanguageId == languageId)
                             .ToList();
            }


            /* Sort the manga list */
            IEnumerable<Manga> orderedMangaList;

            if (orderBy.Length > 0 &&
                typeof(Manga)
                .GetType()
                .GetProperty(orderBy,
                    BindingFlags.IgnoreCase
                    | BindingFlags.Public
                    | BindingFlags.Instance) != null)
            {
                orderedMangaList = mangaList
                                   .OrderByDescending(m =>
                                       typeof(Manga)
                                       .GetProperty(orderBy,
                                       BindingFlags.IgnoreCase
                                       | BindingFlags.Public
                                       | BindingFlags.Instance)
                                       .GetValue(m, null));
            }
            else
            {
                orderedMangaList = mangaList
                                    .OrderByDescending(m => m.Date);
            }

            if (order.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ||
                order.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                orderedMangaList = orderedMangaList.Reverse();
            }

            /* Paginate the manga list */
            if (pageSize > 0 && pageNumber > 0)
            {

                orderedMangaList = orderedMangaList
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize);
            }

            /* Convert to MangaDTO list */
            var mangaDTOList = orderedMangaList
                                .Select(m => new MangaDTO
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

            /* Get manga tags */

            foreach (var manga in mangaDTOList)
            {
                manga.Tags = db.Tags
                             .Where(t => t.MangaId == manga.Id)
                             .Select(t => t.Name)
                             .ToList();
            }

            /* Get pagination info */

            var totalCount = mangaList.Count();
            var totalPages = pageSize > 0 && pageNumber > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 1;

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