using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SchoolManager.WebUI.Controllers
{
    public class GalleryController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Gallery/
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Index()
        {
            return View(db.Galleries.ToList());
        }

        //
        // GET: /Galerias/

        public ActionResult Galerias()
        {
            return View(db.Galleries.ToList());
        }

        //
        // GET: /Gallery/Details/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Details(int id = 0)
        {
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        //
        // GET: /Gallery/Create
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Gallery/Create
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost]
        public ActionResult Create(Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                gallery.CreationDate = DateTime.Now;
                db.Galleries.Add(gallery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gallery);
        }

        //
        // GET: /Gallery/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Edit(int id = 0)
        {
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }

            return View(gallery);
        }

        //
        // POST: /Gallery/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost]
        public ActionResult Edit(Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                gallery.UpdateDate = DateTime.Now;
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }

        //
        // GET: /Gallery/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Delete(int id = 0)
        {
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        //
        // POST: /Gallery/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Gallery gallery = db.Galleries.Find(id);
            db.Galleries.Remove(gallery);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}