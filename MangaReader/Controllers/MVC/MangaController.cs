using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MangaReader.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Web.Configuration;

namespace MangaReader.Controllers.MVC
{
    public class MangaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly string MANGA_DIR = WebConfigurationManager.AppSettings["MangaDir"];
        private readonly string BACKUP_DIR = WebConfigurationManager.AppSettings["BackupDir"];
        private readonly int MAX_PAGE_WIDTH = Int32.Parse(WebConfigurationManager.AppSettings["MaxPageWidth"]);
        private readonly int THUMBNAIL_WIDTH = Int32.Parse(WebConfigurationManager.AppSettings["ThumbnailWidth"]);
        private readonly int THUMBNAIL_HEIGHT = Int32.Parse(WebConfigurationManager.AppSettings["ThumbnailHeight"]);
        private readonly int PREVIEW_WIDTH = Int32.Parse(WebConfigurationManager.AppSettings["PreviewWidth"]);
        private readonly int PREVIEW_HEIGHT = Int32.Parse(WebConfigurationManager.AppSettings["PreviewHeight"]);

        // GET: Manga
        public ActionResult Index()
        {
            var manga = db.Manga.Include(m => m.Artist).Include(m => m.Language).Include(m => m.Collection).Include(m => m.Series);
            return View(manga.ToList());
        }

        // GET: Manga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            if (manga == null)
            {
                return HttpNotFound();
            }
            return View(manga);
        }

        // GET: Manga/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name");
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name");
            ViewBag.SeriesId = new SelectList(db.Series, "Id", "Name");
            return View();
        }

        // POST: Manga/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Date,SeriesId,CollectionId,ArtistId,LanguageId,PageCount,Path")] Manga manga)
        {
            if (ModelState.IsValid)
            {
                db.Manga.Add(manga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", manga.ArtistId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", manga.LanguageId);
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", manga.CollectionId);
            ViewBag.SeriesId = new SelectList(db.Series, "Id", "Name", manga.SeriesId);
            return View(manga);
        }

        // GET: Manga/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            if (manga == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", manga.ArtistId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", manga.LanguageId);
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", manga.CollectionId);
            ViewBag.SeriesId = new SelectList(db.Series, "Id", "Name", manga.SeriesId);
            return View(manga);
        }

        // POST: Manga/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Date,SeriesId,CollectionId,ArtistId,LanguageId,PageCount,Path")] Manga manga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", manga.ArtistId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", manga.LanguageId);
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", manga.CollectionId);
            ViewBag.SeriesId = new SelectList(db.Series, "Id", "Name", manga.SeriesId);
            return View(manga);
        }

        // GET: Manga/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            if (manga == null)
            {
                return HttpNotFound();
            }
            return View(manga);
        }

        // POST: Manga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manga manga = db.Manga.Find(id);
            db.Manga.Remove(manga);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: MangaUpload
        public ActionResult Upload()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name");
            ViewBag.SeriesId = new SelectList(db.Series, "Id", "Name");
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name");
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");

            return View();
        }

        // POST: Manga/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(MangaUploadViewModel model)
        {
            if (model.FileUpload != null)
            {
                if (Path.GetExtension(model.FileUpload.FileName) != ".zip")
                {
                    ModelState.AddModelError("FileUpload", "File must be a zip file.");
                }
            }

            if (ModelState.IsValid)
            {
                // Create manga
                var manga = new Manga();
                manga.Name = model.Name;
                manga.ArtistId = model.ArtistId;
                manga.SeriesId = model.SeriesId;
                manga.CollectionId = model.CollectionId;
                manga.LanguageId = model.LanguageId;
                manga.Date = DateTime.Now;
                manga.PageCount = 0;
                manga.Path = "";

                db.Manga.Add(manga);

                // Create tags
                if (model.Tags != null && model.Tags.Length > 0)
                {
                    var tagList = model.Tags.Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var t in tagList)
                    {
                        var tag = new Tag() { Name = t, MangaId = manga.Id };
                        db.Tags.Add(tag);
                    }
                }

                // Save to DB
                db.SaveChanges();
                


                // Save archive to server
                var backupDir = Server.MapPath(BACKUP_DIR);
                var saveDir = Path.Combine(Server.MapPath(MANGA_DIR), manga.Id.ToString());
                var filePath = Path.Combine(backupDir, model.FileUpload.FileName);

                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }
                model.FileUpload.SaveAs(filePath);

                manga.Path = MANGA_DIR + "/" + manga.Id.ToString();

                // Extract archive contents
                manga.PageCount = UnzipManga(filePath, saveDir);

                // Save to DB
                db.SaveChanges();

                if (manga.PageCount > 0)
                {
                    if (model.Optimize)
                    {
                        OptimizePages(saveDir);
                    }

                    CreatePagePreviews(saveDir);
                    CreateThumbnail(Path.Combine(saveDir, "1.jpg"));
                }

            }

            // Update form controls
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", model.ArtistId);
            ViewBag.SeriesId = new SelectList(db.Series, "Id", "Name", model.SeriesId);
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", model.CollectionId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", model.LanguageId);

            return View(model);
        }

        private int UnzipManga(string zipPath, string extractPath)
        {
            int pageCount = 0;

            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    pageCount++;
                    entry.ExtractToFile(Path.Combine(extractPath, pageCount + Path.GetExtension(entry.FullName)), true);
                }
            }
            return pageCount;
        }
            
        private void SaveAsNewSize(string imgPath, int width, int height, string name)
        {
            using (var origImg = Image.FromFile(imgPath, true))
            {
                using (var img = new Bitmap(origImg, new Size(width, height)))
                {
                    img.Save(Path.Combine(Path.GetDirectoryName(imgPath), name + ".jpg"), ImageFormat.Jpeg);
                }
            }
        }

        private void CreateThumbnail(string imgPath)
        {
            SaveAsNewSize(imgPath, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT, "thumbnail");
        }

        private void CreatePagePreviews(string dir)
        {
            string[] imgPaths = Directory.GetFiles(dir);
            foreach (var imgPath in imgPaths){
                SaveAsNewSize(imgPath, PREVIEW_WIDTH, PREVIEW_HEIGHT, Path.GetFileNameWithoutExtension(imgPath) + "p");
            }
        }

        private void OptimizePages(string dir)
        {
            string[] imgPaths = Directory.GetFiles(dir);
            foreach (var imgPath in imgPaths)
            {
                var savePath = imgPath;
                var origImg = Image.FromFile(imgPath, true);
                Image img = new Bitmap(origImg);
                origImg.Dispose();

                // resize
                if (img.Width > MAX_PAGE_WIDTH)
                {
                    img = ResizeImageFixedWidth(img, MAX_PAGE_WIDTH);
                }

                // change format
                if (!img.RawFormat.Equals(ImageFormat.Jpeg))
                {
                    savePath = Path.Combine(dir, Path.GetFileNameWithoutExtension(imgPath) + ".jpg");
                    System.IO.File.Delete(imgPath);
                }

                img.Save(savePath, ImageFormat.Jpeg);
                img.Dispose();
            }

        }

        public Image ResizeImageFixedWidth(Image imgToResize, int width)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = ((float)width / (float)sourceWidth);

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);

            using (Graphics g = Graphics.FromImage(b))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            }

            imgToResize.Dispose();
            return (Image)b;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
