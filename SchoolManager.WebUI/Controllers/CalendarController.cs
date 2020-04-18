using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace SchoolManager.WebUI.Controllers
{
    public class CalendarController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            ViewBag.Classrooms = this.getClassroomsForUser();
            return View();
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
    }
}
