using System.Web.Mvc;
using SchoolManager.Domain;
using System.IO;
using SchoolManager.Domain.Entities;
using System.Linq;

namespace SchoolManager.WebUI.Controllers
{
    public class RegistrationRequestController : Controller
    {
        private Context db = new Context();

        //
        // GET: /RegistrationRequest/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult Create()
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

        [Authorize(Roles = "SuperAdmin, Admin, Teacher")]
        public ActionResult printEmptyRegistrationRequest()
        {
            using (PdfController pdf = new PdfController())
            {
                return pdf.DescargarPDF(this, "pdfRegistrationRequest");
            }
        }
    }
}
