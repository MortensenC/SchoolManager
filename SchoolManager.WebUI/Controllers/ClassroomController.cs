using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SchoolManager.WebUI.Controllers
{
    public class ClassroomController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Classroom/
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Index()
        {
            return View(db.Classrooms.ToList());
        }

        //
        // GET: /Classroom/Details/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Details(int id = 0)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        //
        // GET: /Classroom/Create
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Classroom/Create

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.Classrooms.Add(classroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classroom);
        }

        //
        // GET: /Classroom/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Edit(int id = 0)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        //
        // POST: /Classroom/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost]
        public ActionResult Edit(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classroom);
        }

        //
        // GET: /Classroom/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Delete(int id = 0)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        //
        // POST: /Classroom/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Classroom classroom = db.Classrooms.Find(id);
            db.Classrooms.Remove(classroom);
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