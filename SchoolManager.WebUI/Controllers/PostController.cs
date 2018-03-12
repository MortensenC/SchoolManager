using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using SchoolManager.WebUI.Helpers.MultipleButtonAttribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SchoolManager.WebUI.Controllers
{
    public class PostController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Post/
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Index()
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View();
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

        private List<Post> GetPostsByUserAdmin()
        {
            List<Post> posts = null;

            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
            {
                posts = db.Posts.ToList();
            }
            else
            {
                var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                posts = db.Posts.Where(p => p.User.Id == user.Id).ToList();
            }

            return posts.OrderBy(p => p.Date).Reverse().ToList();
        }

        //
        // GET: /Post/Novedades

        public ActionResult Novedades(int id = 0)
        {
            ViewBag.Message = "Novedades";
            ViewBag.Id = id;
            ViewBag.PostCount = this.getPostsForUser().Count();
            ViewBag.PageCount = Math.Ceiling((Decimal)ViewBag.PostCount / 8);
            ViewBag.Classroom = this.getClassroomForUser();
            ViewBag.Classrooms = this.getClassroomsForUser();
            return View();
        }

        private Classroom getClassroomForUser()
        {
            Classroom classroom = null;

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Student"))
                {
                    var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                    classroom = user.Classroom;
                }
            }
            
            return classroom;
        }

        //
        // GET: /Post/Details/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Details(int id = 0)
        {
            var post = this.getPostsForUser().Where(p => p.Id == id).First();

            if (post == null)
            {
                return HttpNotFound();
            }

            this.GetFiles(post);
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(post);
        }

        public ActionResult Preview(int page)
        {
            List<Post> returnPosts = null;
            var posts = this.getPostsForUser();
            if (posts.Count > 8)
            {
                if (posts.Count() >= page * 8)
                {
                    returnPosts = posts.GetRange((page - 1) * 8, 8).ToList();
                }
                else
                {
                    returnPosts = posts.GetRange((page - 1) * 8, posts.Count() - (page - 1) * 8).ToList();
                }
            }
            else
            {
                returnPosts = posts;
            }

            if (returnPosts != null)
            {
                this.GetFiles(returnPosts);
            }

            return returnPosts != null ? PartialView(returnPosts) : null;
        }

        private void GetFiles(List<Post> posts)
        {
            foreach (var post in posts)
            {
                this.GetFiles(post);
            }
        }

        private void GetFiles(Post post)
        {
            var path = Server.MapPath("~/Uploads/Posts/" + post.Id);
            if (Directory.Exists(path))
            {
                post.Files = new List<string>();

                foreach (var file in Directory.GetFiles(path))
                {
                    var fileName = file.Split('\\').Last();
                    if (fileName.StartsWith("photo"))
                    {
                        post.Photo = "~/Uploads/Posts/" + post.Id + "/" + fileName;
                    }
                    else
                    {
                        post.Files.Add(fileName);
                    }
                }
            }
        }

        public FileResult Download(string file, int id)
        {
            var path = Server.MapPath("~/Uploads/Posts/" + id + "/" + file);
            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        public ActionResult GetById(int id)
        {
            var post = this.getPostsForUser().Where(p => p.Id == id).First();

            this.GetFiles(post);

            return post != null ? PartialView(post) : null;
        }

        public ActionResult Search(string text, int category, int subject, int classroom, bool mine)
        {
            var posts = this.getPostsForUser();

            if (!string.IsNullOrEmpty(text))
            {
                posts = posts.Where(p => p.Text.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || p.Title.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (category != 0)
            {
                posts = posts.Where(p => p.Category == category).ToList();
            }

            if (subject != 0)
            {
                posts = posts.Where(p => p.Subject == subject).ToList();
            }

            if (classroom != 0)
            {
                posts = posts.Where(p => p.Classroom != null && p.Classroom.Id == classroom).ToList();
            }

            if (mine)
            {
                posts = posts.Where(p => p.User.Id == int.Parse(User.Identity.Name.Split('|')[0])).ToList();
            }

            return PartialView(posts);
        }

        [Authorize]
        public ActionResult SearchPosts(string text, string category = null)
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            if (user == null)
            {
                return HttpNotFound();
            }
            var posts = this.GetPostsByUserAdmin();
            if (!string.IsNullOrEmpty(text))
            {
                posts = posts.Where(p => p.Text.Contains(text) || p.Title.Contains(text)).ToList();
            }

            if (!string.IsNullOrEmpty(category))
            {
                posts = posts.Where(p => p.Category == int.Parse(category)).ToList();
            }

            return PartialView(posts);
        }

        private List<Post> getPostsForUser()
        {
            List<Post> posts = null;

            if (!User.Identity.IsAuthenticated)
            {
                posts = db.Posts.Where(p => p.Public).ToList();
            }
            else
            {
                if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Teacher"))
                {
                    posts = db.Posts.ToList();
                }
                else
                {
                    var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                    if (User.IsInRole("Father"))
                    {
                        var classrooms = db.Users.Where(s => s.MomId == user.Id || s.DadId == user.Id).Select(son => son.Classroom.Id).ToList();

                        posts = db.Posts.Where(p => p.Public || (!p.Public && p.Classroom != null && classrooms.Contains(p.Classroom.Id))).ToList();
                    }
                    else
                    {
                        posts = db.Posts.Where(p => p.Public || (!p.Public && p.Classroom != null && user.Classroom.Id == p.Classroom.Id)).ToList();
                    }
                }
            }

            return posts.OrderBy(p => p.Date).Reverse().ToList();
        }

        private List<Classroom> getClassroomsForUser()
        {
            List<Classroom> classrooms = null;

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Teacher"))
                {
                    classrooms = db.Classrooms.ToList();
                }
                else
                {
                    var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                    if (User.IsInRole("Father"))
                    {
                        classrooms = db.Users.Where(s => s.MomId == user.Id || s.DadId == user.Id).Select(son => son.Classroom).ToList();
                    }
                    else
                    {
                        classrooms = new List<Classroom>() { user.Classroom };
                    }
                }
            }

            return classrooms;
        }

        //
        // GET: /Post/Create
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create()
        {
            ViewBag.Classrooms = db.Classrooms.ToList();
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View();
        }

        //
        // POST: /Post/Create
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Post post, HttpPostedFileBase file, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(post.ClassroomId))
                {
                    post.Classroom = this.db.Classrooms.Find(int.Parse(post.ClassroomId));
                }

                if (string.IsNullOrEmpty(post.Text)) post.Text = string.Empty;

                post.Date = DateTime.Now;
                post.User = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                db.Posts.Add(post);
                db.SaveChanges();

                this.SavePicture(photo, post);
                this.SaveFile(file, post);
                return RedirectToAction("Index");
            }

            ViewBag.Classrooms = db.Classrooms.ToList();
            return View(post);
        }

        private void SavePicture(HttpPostedFileBase file, Post post)
        {
            if (file != null && file.ContentLength > 0)
            {
                var map = Server.MapPath("~/Uploads/Posts") + "/" + post.Id;
                var path = Path.Combine(map, "photo" + Path.GetExtension(file.FileName));

                if (!Directory.Exists(map))
                {
                    Directory.CreateDirectory(map);
                }

                // Code for saving the image file to a physical location.
                var img = new WebImage(file.InputStream);
                if (img.Width > 700 || img.Height > 700)
                {
                    img.Resize(700, 700);
                }

                img.Save(path);
            }
        }

        private void SaveFile(HttpPostedFileBase file, Post post)
        {
            if (file != null && file.ContentLength > 0)
            {
                var map = Server.MapPath("~/Uploads/Posts") + "/" + post.Id;
                var path = Path.Combine(map, file.FileName);

                if (!Directory.Exists(map))
                {
                    Directory.CreateDirectory(map);
                }

                // Code for saving the image file to a physical location.
                file.SaveAs(path);
            }
        }

        //
        // GET: /Post/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpGet]
        //[MultipleButton(Name = "Get", Argument = "Edit")]
        public ActionResult Edit(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            this.GetFiles(post);
            this.SetEditViewBag(post);
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(post);
        }

        //
        // POST: /Post/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost]
        [ValidateInput(false)]
        [MultipleButton(Name = "action", Argument = "Edit")]
        public ActionResult Edit(Post post, HttpPostedFileBase file, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(post.Text)) post.Text = string.Empty;

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();

                this.SavePicture(photo, post);
                this.SaveFile(file, post);

                if (!string.IsNullOrEmpty(post.ClassroomId))
                {
                    db.Dispose();
                    db = new Context();
                    var newPost = db.Posts.Find(post.Id);
                    newPost.Classroom = this.db.Classrooms.Find(int.Parse(post.ClassroomId));
                    db.Entry(newPost).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            this.SetEditViewBag(post);
            return View(post);
        }

        private void SetEditViewBag(Post post)
        {
            post.ClassroomId = post.Classroom != null ? post.Classroom.Id.ToString() : string.Empty;

            ViewBag.Classrooms = db.Classrooms.ToList();
        }

        //
        // GET: /Post/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Delete(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Post/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();

            var map = Server.MapPath("~/Uploads/Posts") + "/" + post.Id;

            if (Directory.Exists(map))
                Directory.Delete(map, true);

            return RedirectToAction("Index");
        }

        //
        // POST: /Post/DeleteFile/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [MultipleButton(Name = "action", Argument = "DeleteFile")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteFile(Post post, String file)
        {
            var map = Server.MapPath("~/Uploads/Posts") + "/" + post.Id;

            if (Directory.Exists(map))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(map);

                foreach (FileInfo f in di.GetFiles().Where(f => f.Name == file))
                {
                    f.Delete();
                }
                //foreach (DirectoryInfo dir in di.GetDirectories())
                //{
                //    dir.Delete(true);
                //}
            }

            return RedirectToAction("Edit", new { post.Id });
        }

        //
        // POST: /Post/DeleteBatch/
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost, ActionName("DeleteBatch")]
        public ActionResult DeleteBatch(List<Post> posts)
        {
            foreach (var post in posts.Where(p => p.Selected))
            {
                db.Posts.Remove(db.Posts.Find(post.Id));
                db.SaveChanges();

                var map = Server.MapPath("~/Uploads/Posts") + "/" + post.Id;

                if (Directory.Exists(map))
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