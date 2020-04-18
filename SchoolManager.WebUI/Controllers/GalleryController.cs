using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(db.Galleries.ToList());
        }

        private void GetPicture(User user)
        {
            var path = Path.Combine(Server.MapPath("~/Uploads/Profile"), user.Username);

            if (Directory.Exists(path))
            {
                var file = Directory.GetFiles(path).FirstOrDefault();
                if (string.IsNullOrEmpty(file))
                {
                    ViewBag.ProfilePicturePath = "../../Content/Images/Profile.jpg";
                    ViewBag.ProfilePicturePathWithOutSlash = "../../Content/Images/Profile.jpg";
                }
                else
                {
                    ViewBag.ProfilePicturePath = "~/Uploads/Profile/" + user.Username + "/" + file.Split('\\').LastOrDefault();
                    ViewBag.ProfilePicturePathWithOutSlash = "/Uploads/Profile/" + user.Username + "/" + file.Split('\\').LastOrDefault();
                }
            }
            else
            {
                ViewBag.ProfilePicturePath = "../../Content/Images/Profile.jpg";
                ViewBag.ProfilePicturePathWithOutSlash = "../../Content/Images/Profile.jpg";
            }
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
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(gallery);
        }

        //
        // GET: /Gallery/Create
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create()
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
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
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
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