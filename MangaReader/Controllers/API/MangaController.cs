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

namespace MangaReader.Controllers.API
{
    [RoutePrefix("api/manga")]
    public class MangaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/manga
        [Route("")]
        public IQueryable<MangaDTO> GetManga()
        {
          //  return db.Manga;
            var manga = from m in db.Manga
                        select new MangaDTO()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            Series = m.Series.Name,
                            Collection = m.Collection.Name,
                            Artist = m.Artist.Name,
                            Language = m.Language.Name,
                            PageCount = m.PageCount,
                            Path = m.Path,
                            Date = m.Date
                        };
            return manga;
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
                Series = m.Series.Name,
                Collection = m.Collection.Name,
                Artist = m.Artist.Name,
                Language = m.Language.Name,
                PageCount = m.PageCount,
                Path = m.Path,
                Date = m.Date
            };

            return Ok(manga);
        }

        // GET: api/manga/page/pageSize/pageNumber/orderBy(optional)
        [Route("page/{pageSize:int}/{pageNumber:int}")]
        public IHttpActionResult GetPage(int pageSize, int pageNumber)
        {
            var totalCount = db.Manga.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var mangaQuery = db.Manga.ToList();

            var mangaList = mangaQuery
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(m => new MangaDTO
                            {
                                Id = m.Id,
                                Name = m.Name,
                                Series = m.Series.Name,
                                Collection = m.Collection.Name,
                                Artist = m.Artist.Name,
                                Language = m.Language.Name,
                                PageCount = m.PageCount,
                                Path = m.Path,
                                Date = m.Date
                            });
                              
            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                MangaList = mangaList
            }; 
            return Ok(result);
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