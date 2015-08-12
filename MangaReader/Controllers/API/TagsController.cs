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

    [RoutePrefix("api/tags")]
    public class TagsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/tags
        [Route("")]
        public IHttpActionResult GetTagNames(string order = "")
        {
            IEnumerable<string> tagList = db.Tags
                                            .DistinctBy(t => t.Name)
                                            .Select(t => t.Name)
                                            .OrderByDescending(t => t);

            if (order.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ||
               order.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                tagList = tagList.Reverse();
            }

            return Ok(tagList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TagExists(int id)
        {
            return db.Tags.Count(e => e.Id == id) > 0;
        }
    }
}