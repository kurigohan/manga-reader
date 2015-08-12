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

    [RoutePrefix("api/tags")]
    public class TagsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/tags
        [Route("")]
        public IHttpActionResult GetTagNames(string order = "")
        {
            var tagList = db.Tags.ToList();
            IEnumerable<TagDTO> orderedTagList;

            orderedTagList = tagList
                                .GroupBy(t => t.Name)
                                .Select(g => new TagDTO() { Name = g.First().Name })
                                .OrderByDescending(t => t.Name);


            if (order.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ||
               order.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                orderedTagList = orderedTagList.Reverse();
            }

            return Ok(orderedTagList);
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