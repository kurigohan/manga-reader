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
    [RoutePrefix("api/series")]
    public class SeriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/series?orderBy&order
        [Route("")]
        public IHttpActionResult GetSeriesList(string order = "", string orderBy = "")
        {
            var seriesList = db.Series.ToList();
            IEnumerable<Series> orderedSeriesList;

            if (orderBy.Length > 0 &&
                typeof(Series)
                .GetType()
                .GetProperty(orderBy,
                    BindingFlags.IgnoreCase
                    | BindingFlags.Public
                    | BindingFlags.Instance) != null)
            {
                orderedSeriesList = seriesList
                                   .OrderByDescending(s =>
                                       typeof(Series)
                                       .GetProperty(orderBy,
                                       BindingFlags.IgnoreCase
                                       | BindingFlags.Public
                                       | BindingFlags.Instance)
                                       .GetValue(s, null));
            }
            else
            {
                orderedSeriesList = seriesList
                                    .OrderByDescending(a => a.Id);
            }

            if (order.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ||
                order.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                orderedSeriesList = orderedSeriesList.Reverse();
            }

            return Ok(orderedSeriesList);
        }

        // GET: api/Series/5
        [ResponseType(typeof(Series))]
        public IHttpActionResult GetSeries(int id)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return NotFound();
            }

            return Ok(series);
        }

        // PUT: api/Series/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSeries(int id, Series series)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != series.Id)
            {
                return BadRequest();
            }

            db.Entry(series).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeriesExists(id))
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

        // POST: api/Series
        [ResponseType(typeof(Series))]
        public IHttpActionResult PostSeries(Series series)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Series.Add(series);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = series.Id }, series);
        }

        // DELETE: api/Series/5
        [ResponseType(typeof(Series))]
        public IHttpActionResult DeleteSeries(int id)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return NotFound();
            }

            db.Series.Remove(series);
            db.SaveChanges();

            return Ok(series);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SeriesExists(int id)
        {
            return db.Series.Count(e => e.Id == id) > 0;
        }
    }
}