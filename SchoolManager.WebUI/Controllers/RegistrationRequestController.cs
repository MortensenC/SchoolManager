using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SchoolManager.WebUI.Controllers
{
    public class RegistrationRequestController : Controller
    {

        private Context db = new Context();
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        //
        // GET: /RegistrationRequest
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                this.GetPicture(user);
            }
            ViewBag.IsRegistrationRequestEnable = db.SystemConfigurations.Find("IsRegistrationRequestEnable").Value.ToUpper().Equals("TRUE");
            //ViewBag.RegistrationRequests = new List() { };
            return View(db.RegistrationRequests.Where(rr => rr.Status != "Aprobada" && rr.Status != "Rechazada").ToList());
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
        // GET: /RegistrationRequest/Details/5

        public ActionResult Details(int id)
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            return View();
        }

        //
        // GET: /RegistrationRequest/Create

        public ActionResult Create(string dni = "")
        {
            if (User.Identity.IsAuthenticated) {
                var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                this.GetPicture(user);
            }
            if (string.IsNullOrEmpty(dni))
                return View();
            else
            {
                ViewBag.DNI = dni;
                return View();
            }
        }

        //
        // POST: /RegistrationRequest/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin, Teacher, Father")]
        public ActionResult Create(RegistrationRequest RegistrationRequest)
        {
            //log.Info("Trying to create a new RegistrationRequest entity.");
            try
            {

                if (ModelState.IsValid)
                {
                    RegistrationRequest rr = null;

                    if (!String.IsNullOrEmpty(RegistrationRequest.DNI))
                        rr = db.RegistrationRequests.Where<RegistrationRequest>(r => r.DNI.ToUpper().Equals(RegistrationRequest.DNI.ToUpper())).FirstOrDefault<RegistrationRequest>();

                    if (rr != null)
                    {
                        //log.Error("Ya existe una solicitud de inscripción con el mismo DNI");
                        return View(RegistrationRequest);
                    }

                    AnalizarEstadoSolicitud(RegistrationRequest);
                    db.RegistrationRequests.Add(RegistrationRequest);

                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                //log.Error("Ah ocurrido un error intentando guardar la solicitud inscripción", e);
            }

            return View(RegistrationRequest);
        }
        /// <summary>
        /// Analiza los datos que han sido cargados en la solicitud y le define un estado que se almacena en el valor status de la solicitud
        /// </summary>
        /// <param name="RegistrationRequest"></param>
        private void AnalizarEstadoSolicitud(RegistrationRequest RegistrationRequest)
        {
            if (RegistrationRequest.EstaCompleta())
                RegistrationRequest.setCompleto();
            else
                RegistrationRequest.setFaltanDatos();
        }

        //
        // GET: /RegistrationRequest/Edit/5

        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            var rr = db.RegistrationRequests.Find(id);
            return View("Edit",rr);
        }

        //
        // POST: /RegistrationRequest/Edit/5

        [HttpPost]
        public ActionResult Edit(RegistrationRequest rr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rr).State = EntityState.Modified;
                AnalizarEstadoSolicitud(rr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rr);
        }

        public ActionResult Aprove(int id)
        {
            var rr = db.RegistrationRequests.Find(id);

            db.Entry(rr).State = EntityState.Modified;
            rr.setAprobada();
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Decline(int id)
        {
            var rr = db.RegistrationRequests.Find(id);

            db.Entry(rr).State = EntityState.Modified;
            rr.setRechazada();
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /RegistrationRequest/Delete/5

        public ActionResult Delete(int id)
        {
            var rr = db.RegistrationRequests.Find(id);
            db.RegistrationRequests.Remove(rr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /RegistrationRequest/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult printEmptyRegistrationRequest(object id)
        {

            using (var pdfController = new PdfController())
            {
                int result;
                RegistrationRequest rr = null;
                if (Int32.TryParse(id.ToString(), out result))
                    rr = db.RegistrationRequests.Find(Int32.Parse(id.ToString()));
                else
                    rr = new RegistrationRequest();
                return pdfController.DescargarPDF(this, "pdfRegistrationRequest", rr);
            }
            
        }

        public ActionResult EnableRegistrationRequest()
        {
            var rr = db.SystemConfigurations.Find("IsRegistrationRequestEnable");

            db.Entry(rr).State = EntityState.Modified;
            rr.Value = "TRUE";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DisableRegistrationRequest()
        {
            var rr = db.SystemConfigurations.Find("IsRegistrationRequestEnable");

            db.Entry(rr).State = EntityState.Modified;
            rr.Value = "FALSE";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SearchDNI(String dni)
        {
            //log.Info("Trying to search for a RegistrationRequest entity.");
            try
            {
                // TODO: Add insert logic here
                var rr = db.RegistrationRequests.Where<RegistrationRequest>(r => r.DNI.ToUpper().Equals(dni.ToUpper())).FirstOrDefault<RegistrationRequest>();
                if (rr == null)
                {
                    return RedirectToAction("Create", new { dni = dni });
                }
                else
                {
                    if (rr.FaltanDatos() || rr.AControlar())
                        return RedirectToAction("Edit", new { id = rr.Id });
                    else
                    {
                        string _message = String.Empty;
                        if (rr.EstaAprobada())
                            _message = "La solicitud ya está aprobada.";
                        else if(rr.EstaRechazada())
                            _message = "La solicitud fue rechazada.";
                        return RedirectToAction("Search", new { message = _message });
                    }
                    

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Search(string message = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
                this.GetPicture(user);
            }
            ViewBag.message = message;
            return View();
        }
    }
}
