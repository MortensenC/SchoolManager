using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SchoolManager.WebUI.Controllers
{
    [HandleError()]
    public class BookController : Controller
    {
        private Context db = new Context();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Book/
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Index()
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(db.Books.ToList());
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
            }
        }

        //
        // GET: /Book/Biblioteca
        [AllowAnonymous]
        public ActionResult Biblioteca()
        {
            var books = db.Books.Where(b => b.Highlighted).ToList();

            ViewBag.PictureMaps = new Dictionary<int, string>();
            foreach (var book in books)
            {
                var map = Server.MapPath("~/Uploads/Books") + "/" + book.Id;

                if (Directory.Exists(map))
                {
                    ViewBag.PictureMaps[book.Id] = "~/Uploads/Books/" + book.Id + "/" + Directory.GetFiles(map).First().Split('\\').Last();
                }
                else
                {
                    ViewBag.PictureMaps[book.Id] = "../../Content/Images/Book.jpg";
                }
            }

            return View(books);
        }

        [AllowAnonymous]
        public ActionResult Search(string text)
        {
            List<Book> books = new List<Book>();
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    books = db.Books.Where(b => !b.Highlighted).Take(20).ToList();
                }
                else
                {
                    books =
                        db.Books.Where(
                            b =>
                            !b.Highlighted &&
                            (b.Title.ToLower().Contains(text.ToLower()) ||
                             b.Description.ToLower().Contains(text.ToLower()) ||
                             b.Author.ToLower().Contains(text.ToLower()))).ToList();
                }
            }
            catch (Exception e)
            {
                books.Add(new Book() { Title = e.Message, Description = e.InnerException.Message });
            }

            return PartialView(books);
        }

        //
        // GET: /Book/Details/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Details(int id = 0)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        //
        // GET: /Book/Create
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create()
        {
            ViewBag.Users = db.Users.ToList();
            return View();
        }

        //
        // POST: /Book/Create

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create(Book book, HttpPostedFileBase photo)
        {
            log.Info("Trying to create a new Book entity.");
            try
            {
                if (ModelState.IsValid)
                {
                    if (book.Description == null) book.Description = string.Empty;

                    if (!string.IsNullOrEmpty(book.UserId))
                    {
                        book.User = this.db.Users.Find(int.Parse(book.UserId));
                    }

                    db.Books.Add(book);
                    db.SaveChanges();
                    this.SavePicture(photo, book);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                log.Error("Error creating new Book", e);
            }

            ViewBag.Users = db.Users.ToList();
            return View(book);
        }

        private void SavePicture(HttpPostedFileBase file, Book book)
        {
            if (file != null && file.ContentLength > 0)
            {
                var map = Server.MapPath("~/Uploads/Books") + "/" + book.Id;
                var path = Path.Combine(map, "photo" + Path.GetExtension(file.FileName));

                if (Directory.Exists(map))
                {
                    Directory.Delete(map, true);
                }

                Directory.CreateDirectory(map);

                // Code for saving the image file to a physical location.
                var img = new WebImage(file.InputStream);
                if (img.Width > 500 || img.Height > 500)
                {
                    img.Resize(500, 500);
                }

                img.Save(path);
            }
        }

        //
        // GET: /Book/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Users = db.Users.ToList();
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        //
        // POST: /Book/Edit/5

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Edit(Book book, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                book.User = !string.IsNullOrEmpty(book.UserId) ? this.db.Users.Find(int.Parse(book.UserId)) : null;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                this.SavePicture(photo, book);
                return RedirectToAction("Index");
            }
            return View(book);
        }

        //
        // GET: /Book/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Delete(int id = 0)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        //
        // POST: /Book/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();

            var map = Server.MapPath("~/Uploads/Books") + "/" + book.Id;

            if (Directory.Exists(map))
            {
                Directory.Delete(map, true);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}