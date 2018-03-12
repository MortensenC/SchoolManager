using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManager.WebUI.Controllers
{
    public class RegistrationRequestController : Controller
    {

        private Context db = new Context();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum eLevels { Inicial, EGB, Polimodal, CEF, CEC, Especial, Adultos, Artística, Otro };
        public static IEnumerable<String> eYears = new List<string>() { "1°", "2°", "3°", "4°", "5°", "6°", "7°", "8°", "9°" };
        public enum eTurn { Mañana, Tarde, Jornada_Completa, Vespertino, Noche };
        public enum eLevelInstruction { Ninguno, Primario, Secundario, Terciario, Universitario, Posgrado }
        //public enum eConditional { Sí, No };

        //private void SetViewBagLevel()
        //{
        //    IEnumerable<eLevels> values = Enum.GetValues(typeof(eLevels)).Cast<eLevels>();

        //    IEnumerable<SelectListItem> items =  from value in values
        //        select new SelectListItem
        //        {
        //            Text = value.ToString(),
        //            Value = value.ToString()
        //        };

        //    ViewBag.Level = items;
        //}

        //private void SetViewBagYear()
        //{
        //    IEnumerable<SelectListItem> items = from value in eYears
        //                                        select new SelectListItem
        //                                        {
        //                                            Text = value.ToString(),
        //                                            Value = value.ToString()
        //                                        };

        //    ViewBag.Year = items;
        //}

        //private void SetViewBagTurn()
        //{
        //    IEnumerable<eTurn> values = Enum.GetValues(typeof(eTurn)).Cast<eTurn>();

        //    IEnumerable<SelectListItem> items = from value in values
        //                                        select new SelectListItem
        //                                        {
        //                                            Text = value.ToString().Replace("_"," "),
        //                                            Value = value.ToString()
        //                                        };

        //    ViewBag.Turn = items;
        //}

        //private void SetViewBagLevelInstruction()
        //{
        //    IEnumerable<eLevelInstruction> values = Enum.GetValues(typeof(eLevelInstruction)).Cast<eLevelInstruction>();

        //    IEnumerable<SelectListItem> items = from value in values
        //                                        select new SelectListItem
        //                                        {
        //                                            Text = value.ToString(),
        //                                            Value = value.ToString()
        //                                        };

        //    ViewBag.LevelInstruction = items;
        //}

        //
        // GET: /RegistrationRequest
        public ActionResult Index()
        {
            var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            
            return View(db.RegistrationRequests.ToList());
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
            return View();
        }

        //
        // GET: /RegistrationRequest/Create

        public ActionResult Create()
        {
            //var user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            //this.GetPicture(user);
            //SetViewBagLevel();
            //SetViewBagYear();
            //SetViewBagTurn();
            //SetViewBagLevelInstruction();
            return View();
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
        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create(RegistrationRequest RegistrationRequest)
        {
            log.Info("Trying to create a new RegistrationRequest entity.");
            try
            {

                if (ModelState.IsValid)
                {
                    RegistrationRequest rr = null;
                    //if (RegistrationRequest.Id > 0)
                    //    rr = db.RegistrationRequests.Find(RegistrationRequest.Id);
                    //else 
                    if (!String.IsNullOrEmpty(RegistrationRequest.DNI))
                        rr = db.RegistrationRequests.Where<RegistrationRequest>(r => r.DNI.ToUpper().Equals(RegistrationRequest.DNI.ToUpper())).FirstOrDefault<RegistrationRequest>();

                    if (rr != null)
                    {
                        log.Error("There is another RegistrationRequest for the same DNI");
                        return View(RegistrationRequest);
                    }
                    
                    db.RegistrationRequests.Add(RegistrationRequest);
                    //else
                    //{
                    //    db.Entry(rr).CurrentValues.SetValues(RegistrationRequest);
                    //    db.Entry(rr).State = EntityState.Modified;
                    //}
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                log.Error("Error creating new RegistrationRequest", e);
            }

            return View(RegistrationRequest);
        }

        //
        // GET: /RegistrationRequest/Edit/5

        public ActionResult Edit(int id)
        {
            var rr = db.RegistrationRequests.Find(id);
            return View("Edit",rr);
            //return LoadAndCreate(rr);
        }

        //
        // POST: /RegistrationRequest/Edit/5

        [HttpPost]
        public ActionResult Edit(RegistrationRequest rr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rr);
        }

        //
        // GET: /RegistrationRequest/Delete/5

        public ActionResult Delete(int id)
        {
            //return View();
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
                return pdfController.DescargarPDF(this, "pdfRegistrationRequest", rr);
            }
            
        }

        [HttpPost]
        public ActionResult Search(String dni)
        {
            log.Info("Trying to search for a RegistrationRequest entity.");
            try
            {
                // TODO: Add insert logic here
                var rr = db.RegistrationRequests.Where<RegistrationRequest>(r => r.DNI.ToUpper().Equals(dni.ToUpper())).FirstOrDefault<RegistrationRequest>();
                if (rr == null)
                    return RedirectToAction("Create");
                else
                    return RedirectToAction("Edit",new { id = rr.Id});
            }
            catch(Exception ex)
            {
                throw ex;
                //return View();
            }
        }

        //private ActionResult LoadAndCreate(RegistrationRequest rr)
        //{
        //    return View("Create",rr);
        //}

        public ActionResult Search()
        {
            return View();
        }
    }
}
