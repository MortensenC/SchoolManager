using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManager.WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private Context db = new Context();

        //
        // GET: /User/
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult Index()
        {
            var teachers = db.Roles.Where(r => r.Name == "Teacher").First().Users;
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(teachers);
        }

        //
        // GET: /User/Details/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Details(int id = 0)
        {
            User persona = db.Users.Find(id);

            if (persona == null)
            {
                return HttpNotFound();
            }

            //this.GetPicture(persona);
            SetEditViewBag(persona);

            ViewBag.Dad = db.Users.Find(persona.DadId);
            ViewBag.Mom = db.Users.Find(persona.MomId);

            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(persona);
        }

        //
        // GET: /User/Contacts
        [Authorize]
        public ActionResult Contacts(int ClassroomId = 0)
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            ViewBag.Classrooms = db.Classrooms.ToList();
            if (ClassroomId > 0)
                ViewBag.ClassroomId = ClassroomId;

            this.GetPicture(user);

            return View();
        }

        //[Authorize]
        //public ActionResult Export(string text, string classroom = null)
        //{
        //    var userTable = new DataTable("test");

        //    userTable.Columns.Add("A", typeof(string));
        //    userTable.Columns.Add("B", typeof(string));
        //    userTable.Columns.Add("C", typeof(string));
        //    userTable.Columns.Add("D", typeof(string));
        //    userTable.Columns.Add("E", typeof(int));

        //    //for (int i = 0; i < 500000; i++)
        //    //{
        //    //    userTable.Rows.Add("Lorem ipsum dolor sit amet, consectetur adipiscing elit",
        //    //                       "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
        //    //                       DateTime.Now.ToString(), "Ut enim ad minim", i);
        //    //}

        //    var grid = new GridView();
        //    grid.DataSource = userTable;
        //    grid.DataBind();

        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=repor.xls");

        //    Response.Charset = "";
        //    var sw = new StringWriter();
        //    var htw = new HtmlTextWriter(sw);
        //    grid.RenderControl(htw);

        //    //Response.Output.Write(sw.ToString());
        //    //Response.Flush();
        //    //Response.End();
        //    return Content(sw.ToString(), "application/ms-excel");
        //}

        [Authorize]
        public ActionResult Export(string text, string classroom)
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            var contacts = this.GetContacts(user, classroom, text);
            var classroomName = db.Classrooms.Find(int.Parse(classroom)).Name;

            var userTable = new DataTable("teste");

            userTable.Columns.Add("Nombre", typeof(string));
            userTable.Columns.Add("Email", typeof(string));
            userTable.Columns.Add("Teléfono", typeof(string));
            userTable.Columns.Add("Dirección", typeof(string));
            userTable.Columns.Add("DNI", typeof(string));

            foreach (var usersByClassroom in contacts.GroupBy(u => u.Classroom))
            {
                if (usersByClassroom.Key == null)
                {
                    userTable.Rows.Add("Usuarios sin curso asignado", "", "", "");
                }
                else
                {
                    userTable.Rows.Add(usersByClassroom.Key.Name, "", "", "");
                }

                foreach (var item in usersByClassroom)
                {
                    userTable.Rows.Add(item.FullName, item.Email, item.Phone, item.Address, item.Username);

                    if (item.Dad != null)
                    {
                        userTable.Rows.Add("Padre: " + item.Dad.FullName, item.Dad.Email, item.Dad.Phone, item.Dad.Address, item.Dad.Username);
                    }

                    if (item.Mom != null)
                    {
                        userTable.Rows.Add("Madre: " + item.Mom.FullName, item.Mom.Email, item.Mom.Phone, item.Mom.Address, item.Mom.Username);
                    }
                }
            }

            var grid = new GridView();
            grid.DataSource = userTable;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMdd") + " - " + classroomName + ".xls");
            //Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
            return Content(sw.ToString(), "application/ms-excel");
        }

        [Authorize(Roles = "SuperAdmin, Admin, Teacher, Father")]
        public ActionResult Docentes()
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(db.Roles.Where(r => r.Name == "Teacher").First().Users);
        }

        [Authorize]
        public ActionResult Search(string text, string classroom = null)
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            if (user == null)
            {
                return HttpNotFound();
            }

            //if ((User.IsInRole("Admin") || User.IsInRole("SuperAdmin")) && classroom == null)
            //{
            //    return PartialView(new List<User>());
            //}

            return PartialView(GetContacts(user, classroom, text));
        }

        private List<User> GetContacts(User user, string classroom, string text)
        {
            var contacts = new List<User>();
            var classroomId = 0;
            if (!string.IsNullOrEmpty(classroom))
            {
                classroomId = int.Parse(classroom);
            }

            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Teacher"))
            {
                if (classroomId != 0)
                {
                    contacts = db.Users.Where(u => u.Classroom.Id == classroomId).ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        contacts = db.Users.ToList().Where(c => c.IsInRole("Student") && c.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || c.Surname.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || c.Username.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                }
            }

            if (User.IsInRole("Father"))
            {
                var childs = db.Users.Where(u => u.MomId == user.Id || u.DadId == user.Id).ToList();
                foreach (var child in childs)
                {
                    contacts.AddRange(db.Users.Where(u => u.Classroom.Id == child.Classroom.Id));
                }
            }

            if (User.IsInRole("Student"))
            {
                contacts = db.Users.Where(u => u.Classroom.Id == user.Classroom.Id).ToList();
            }

            if (!string.IsNullOrEmpty(text))
            {
                contacts = contacts.Where(c => c.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || c.Surname.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || c.Username.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (classroomId != 0)
            {
                var cr = db.Classrooms.Find(classroomId);
                if (cr.Name == "Exalumnos")
                {
                    contacts = contacts.OrderBy(u => u.Promotion).Reverse().ToList();
                }
            }

            return this.GetParents(contacts);
        }

        private List<User> GetParents(List<User> users)
        {
            foreach (var user in users)
            {
                if (user.DadId != null && user.DadId != 0)
                {
                    user.Dad = db.Users.Find(user.DadId);
                }

                if (user.MomId != null && user.MomId != 0)
                {
                    user.Mom = db.Users.Find(user.MomId);
                }
            }

            return users;
        }

        //
        // GET: /User/Create
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult Create()
        {
            ViewBag.Classrooms = db.Classrooms.ToList();
            ViewBag.Users = db.Users.OrderBy(u => u.Surname).ToList().Where(u => u.IsInRole("Father")).ToList();
            ViewBag.EditProfilePicturePath = "../../Content/Images/Profile.jpg";
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult Create(User user, HttpPostedFileBase file)
        {
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(user.ClassroomId))
                {
                    user.Classroom = this.db.Classrooms.Find(int.Parse(user.ClassroomId));
                }

                this.UpdateRole(user);

                user.Password = this.CreatePassword(user);
                db.Users.Add(user);
                db.SaveChanges();

                this.SavePicture(file, user);

                if (user.IsInRole("Teacher"))
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Contacts");
            }

            //TODO: acá se está perdiendo el archivo
            ViewBag.Classrooms = db.Classrooms.ToList();
            ViewBag.ProfilePicturePath = "../../Content/Images/Profile.jpg";
            ViewBag.Users = db.Users.OrderBy(u => u.Surname).ToList().Where(u => u.IsInRole("Father")).ToList();

            return View(user);
        }

        private void UpdateRole(User user)
        {
            if (user.UserType.Equals("Student"))
            {
                user.Roles.Add(db.Roles.Where(r => r.Name.Equals("Student")).FirstOrDefault());
            }
            else if (user.UserType.Equals("Father"))
            {
                user.Roles.Add(db.Roles.Where(r => r.Name.Equals("Father")).FirstOrDefault());
            }
            else if (user.UserType.Equals("Teacher"))
            {
                user.Roles.Add(db.Roles.Where(r => r.Name.Equals("Teacher")).FirstOrDefault());
            }
        }

        private string CreatePassword(User user)
        {
            return string.Format("{0}{1}", int.Parse(user.Username[user.Username.Length - 2].ToString()) + int.Parse(user.Username[user.Username.Length - 1].ToString()), char.ToLower(user.Surname[0]));
        }

        //
        // GET: /User/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult Edit(int id = 0)
        {
            User persona = db.Users.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }

            persona.UserType = persona.Roles.Count > 0 ? persona.Roles.FirstOrDefault().Name : string.Empty;

            SetEditViewBag(persona);

            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View(persona);
        }

        //
        // GET: /User/Profile/5
        [Authorize]
        new public ActionResult Profile(int id = 0)
        {
            if (int.Parse(User.Identity.Name.Split('|')[0]) != id)
            {
                return HttpNotFound();
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.UserType = user.Roles.Count > 0 ? user.Roles.FirstOrDefault().Name : string.Empty;

            this.GetPicture(user);

            return View(user);
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

        private string GetPictureEdit(User user)
        {
            var path = Path.Combine(Server.MapPath("~/Uploads/Profile"), user.Username);

            if (Directory.Exists(path))
            {
                var file = Directory.GetFiles(path).FirstOrDefault();
                if (string.IsNullOrEmpty(file))
                {
                    return "../../Content/Images/Profile.jpg";
                    //ViewBag.ProfilePicturePathWithOutSlash = "../../Content/Images/Profile.jpg";
                }
                else
                {
                    return "~/Uploads/Profile/" + user.Username + "/" + file.Split('\\').LastOrDefault();
                    //ViewBag.ProfilePicturePathWithOutSlash = "/Uploads/Profile/" + user.Username + "/" + file.Split('\\').LastOrDefault();
                }
            }
            else
            {
                return "../../Content/Images/Profile.jpg";
                //ViewBag.ProfilePicturePathWithOutSlash = "../../Content/Images/Profile.jpg";
            }
        }

        private void SetEditViewBag(User user)
        {
            user.ClassroomId = user.Classroom != null ? user.Classroom.Id.ToString() : string.Empty;

            ViewBag.Classrooms = db.Classrooms.ToList();
            ViewBag.Users = db.Users.OrderBy(u => u.Surname).ToList().Where(u => u.IsInRole("Father")).ToList();
            ViewBag.EditProfilePicturePath = this.GetPictureEdit(user);
            //this.GetPicture(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult Edit(User user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                this.SavePicture(file, user);

                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();

                db.Dispose();

                db = new Context();

                var newUser = db.Users.Find(user.Id);

                if (!string.IsNullOrEmpty(user.ClassroomId))
                {
                    newUser.Classroom = this.db.Classrooms.Find(int.Parse(user.ClassroomId));
                }
                else
                {
                    if (newUser.Classroom != null)
                    {
                        newUser.Classroom.Users.Remove(newUser);
                        db.Entry(newUser.Classroom).State = EntityState.Modified;
                    }

                    newUser.Classroom = null;
                }

                newUser.UserType = user.UserType;
                newUser.Roles.Clear();

                UpdateRole(newUser);

                db.Entry(newUser).State = EntityState.Modified;

                db.SaveChanges();

                if (newUser.UserType == "Teacher")
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Contacts", new { ClassroomId = user.ClassroomId });
            }

            SetEditViewBag(user);

            return View(user);
        }

        [HttpPost]
        [Authorize]
        public string EditPicture()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var file = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
                var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));

                this.SavePicture(file, user);
                this.GetPicture(user);

                return ((string)ViewBag.ProfilePicturePath).Substring(1);
            }

            return ViewBag.ProfilePicturePath;
        }

        private void SavePicture(HttpPostedFile file, User user)
        {
            if (file != null && file.ContentLength > 0)
            {
                var map = Server.MapPath("~/Uploads/Profile") + "\\" + user.Username;
                var path = Path.Combine(map, "photo" + Path.GetExtension(file.FileName));

                if (Directory.Exists(map))
                {
                    Directory.Delete(map, true);
                }

                // Code for saving the image file to a physical location.
                var img = new WebImage(file.InputStream);
                if (img.Width > 500 || img.Height > 500)
                {
                    img.Resize(500, 500);
                }

                if (!Directory.Exists(map))
                {
                    Directory.CreateDirectory(map);
                }

                img.Save(path);
            }
        }

        private void SavePicture(HttpPostedFileBase file, User user)
        {
            if (file != null && file.ContentLength > 0)
            {
                var map = Server.MapPath("~/Uploads/Profile") + "/" + user.Username;
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

        private void DeletePicture(User user)
        {
            // Code for delete the image for a user
            var path = Server.MapPath("~/Uploads/Profile") + "/" + user.Username;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        //
        // GET: /User/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        //
        // POST: /User/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.Roles.Clear();
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            db.Users.Remove(user);
            db.SaveChanges();
            this.DeletePicture(user);

            if (user.IsInRole("Teacher"))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Contacts");
        }

        //
        // GET: /User/YearMoved
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult YearMoved()
        {
            return View();
        }

        //
        // GET: /User/MoveYear
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult MoveYear()
        {
            return View();
        }

        //
        // POST: /User/MoveYear
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost, ActionName("MoveYear")]
        public ActionResult MoveYearConfirmed()
        {
            //var exalumnos = db.Classrooms.Where(c => c.Name == "Exalumnos").First();
            //var students = db.Users.Where(s => s.Classroom != null && s.Classroom.Id != exalumnos.Id).ToList();
            //var studentsByClassroom = students.GroupBy(s => s.Classroom).Reverse();
            //var nextClassroom = exalumnos;

            //foreach (var studentInClassroom in studentsByClassroom)
            //{
            //    foreach (var student in studentInClassroom)
            //    {
            //        student.Classroom = nextClassroom;
            //        db.Entry(student).State = EntityState.Modified;
            //    }

            //    nextClassroom = studentInClassroom.Key;
            //}

            //db.SaveChanges();
            try
            {
                using (var context = new Context())
                {
                    context.Database.ExecuteSqlCommand(
                        "UPDATE Posts SET Classroom_Id = 7 WHERE Classroom_Id = 6 AND Category <> " + ((int)Category.Tutoriales).ToString() + " AND Category <> " + ((int)Category.Recursos).ToString() + ";" +
                        "UPDATE Posts SET Classroom_Id = 6 WHERE Classroom_Id = 5 AND Category <> " + ((int)Category.Tutoriales).ToString() + " AND Category <> " + ((int)Category.Recursos).ToString() + ";" +
                        "UPDATE Posts SET Classroom_Id = 5 WHERE Classroom_Id = 4 AND Category <> " + ((int)Category.Tutoriales).ToString() + " AND Category <> " + ((int)Category.Recursos).ToString() + ";" +
                        "UPDATE Posts SET Classroom_Id = 4 WHERE Classroom_Id = 3 AND Category <> " + ((int)Category.Tutoriales).ToString() + " AND Category <> " + ((int)Category.Recursos).ToString() + ";" +
                        "UPDATE Posts SET Classroom_Id = 3 WHERE Classroom_Id = 2 AND Category <> " + ((int)Category.Tutoriales).ToString() + " AND Category <> " + ((int)Category.Recursos).ToString() + ";" +
                        "UPDATE Posts SET Classroom_Id = 2 WHERE Classroom_Id = 1 AND Category <> " + ((int)Category.Tutoriales).ToString() + " AND Category <> " + ((int)Category.Recursos).ToString() + ";"
                    );
                }
            }
            catch (Exception)
            {

            }

            try
            {
                using (var context = new Context())
                {
                    context.Database.ExecuteSqlCommand(
                    @"
                    /*** UPDATE Users ***/
                    UPDATE Users SET Classroom_Id = 7 WHERE Classroom_Id = 6;
                    UPDATE Users SET Classroom_Id = 6 WHERE Classroom_Id = 5;
                    UPDATE Users SET Classroom_Id = 5 WHERE Classroom_Id = 4;
                    UPDATE Users SET Classroom_Id = 4 WHERE Classroom_Id = 3;
                    UPDATE Users SET Classroom_Id = 3 WHERE Classroom_Id = 2;
                    UPDATE Users SET Classroom_Id = 2 WHERE Classroom_Id = 1;");

                }
            }
            catch (Exception)
            {

            }

            return RedirectToAction("YearMoved");
        }

        //
        // GET: /User/Reset/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult Reset(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Reset/5

        [HttpPost, ActionName("Reset")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public ActionResult ResetConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.Password = this.CreatePassword(user);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            if (user.IsInRole("Teacher"))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Contacts");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}