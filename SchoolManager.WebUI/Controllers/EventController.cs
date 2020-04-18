using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using SchoolManager.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SchoolManager.WebUI.Controllers
{
    public class EventController : Controller
    {
        private Context db = new Context();

        [HttpGet]
        public JsonResult GetEventsCalendar()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Event> events = new List<Event>();

                var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                if (User.IsInRole("Father"))
                {
                    var classrooms = db.Users.Where(s => s.MomId == user.Id || s.DadId == user.Id).Select(son => son.Classroom.Id).ToList();

                    events = db.Events.Where(e => (e.start >= DateTime.Now || e.start >= DateTime.Now) && e.classroomId.HasValue && classrooms.Contains(e.classroomId.Value)).ToList();
                }
                else if (User.IsInRole("Student"))
                {
                    var classrooms = db.Users.Where(u => u.Id == user.Id).Select(u => u.Classroom.Id).ToList();

                    events = db.Events.Where(e => (e.start >= DateTime.Now || e.start >= DateTime.Now) && e.classroomId.HasValue && classrooms.Contains(e.classroomId.Value)).ToList();
                }
                else if (user.IsInRole("Teacher"))
                {
                    events = db.Events.Where(e => (e.start >= DateTime.Now || e.start >= DateTime.Now) && e.userId == user.Id).ToList();
                }
                else
                {
                    events = db.Events.Where(e => e.start >= DateTime.Now || e.start >= DateTime.Now).ToList();
                }

                var eventsVM = events.Select(e => new EventVM()
                {
                    id = e.Id,
                    title = e.title,
                    description = e.description,
                    url = e.url,
                    start = e.start,
                    end = e.end,
                    editable = e.userId == user.Id,
                    classroomId = e.classroomId,
                    classroomName = e.Classroom?.Name ?? string.Empty,
                    students = e.students
                });

                return new JsonResult { Data = eventsVM, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                throw new Exception("Usuario no logueado");
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            bool status = false;
            try
            {
                if (e.Id > 0)
                {
                    var _event = db.Events.Where(ev => ev.Id == e.Id).FirstOrDefault();
                    if (_event != null)
                    {
                        _event.start = e.start;
                        _event.classroomId = e.classroomId;
                        _event.end = e.end;
                        _event.title = e.title;
                        _event.url = e.url;
                        _event.description = e.description;
                        _event.students = e.students;
                    }
                }
                else
                {
                    var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                    e.userId = user.Id;
                    db.Events.Add(e);
                }

                db.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { status, error = ex.Message } };
            }
            return new JsonResult { Data = new { status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(long eventId)
        {
            var status = false;
            try
            {
                var _event = db.Events.Where(ev => ev.Id == eventId).FirstOrDefault();
                if (_event != null)
                {
                    db.Events.Remove(_event);
                    db.SaveChanges();
                    status = true;
                }
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { status } };
            }
            return new JsonResult { Data = new { status } };
        }
    }
}