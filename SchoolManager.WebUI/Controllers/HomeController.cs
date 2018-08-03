using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();

        [AllowAnonymous]
        public ActionResult Index()
        {
            User user = null;
            if (User.Identity.IsAuthenticated)
                user = db.Users.Find(int.Parse(User.Identity.Name.Split('|')[0]));
            this.GetPicture(user);
            //ViewBag.ProfilePicturePath = "../../Content/Images/Profile.jpg";
            //ViewBag.IsRegistrationRequestEnable = true; // db.SystemConfigurations.Find("IsRegistrationRequestEnable").Value.ToUpper().Equals("TRUE");
            var posts = db.Posts.Where(p => p.Public).OrderByDescending(p => p.Id).Take(3).ToList();

            // Post 1
            if (posts.Count() > 0)
            {
                var post = posts.First();
                ViewBag.Post1 = post;
                var path = Server.MapPath("~/Uploads/Posts/" + post.Id);
                if (Directory.Exists(path))
                {
                    foreach (var fileName in Directory.GetFiles(path).Select(file => file.Split('\\').Last()).Where(fileName => fileName.StartsWith("photo")))
                    {
                        ViewBag.Photo1 = "~/Uploads/Posts/" + post.Id + "/" + fileName;
                    }
                }
            }

            // Post 2
            if (posts.Count() > 1)
            {
                var post = posts.ElementAt(1);
                ViewBag.Post2 = post;
                var path = Server.MapPath("~/Uploads/Posts/" + post.Id);
                if (Directory.Exists(path))
                {
                    foreach (var fileName in Directory.GetFiles(path).Select(file => file.Split('\\').Last()).Where(fileName => fileName.StartsWith("photo")))
                    {
                        ViewBag.Photo2 = "~/Uploads/Posts/" + post.Id + "/" + fileName;
                    }
                }
            }

            // Post 3
            if (posts.Count() > 2)
            {
                var post = posts.ElementAt(2);
                ViewBag.Post3 = post;
                var path = Server.MapPath("~/Uploads/Posts/" + post.Id);
                if (Directory.Exists(path))
                {
                    foreach (var fileName in Directory.GetFiles(path).Select(file => file.Split('\\').Last()).Where(fileName => fileName.StartsWith("photo")))
                    {
                        ViewBag.Photo3 = "~/Uploads/Posts/" + post.Id + "/" + fileName;
                    }
                }
            }

            return View();
        }

        private void GetPicture(User user)
        {
            if (User.Identity.IsAuthenticated)
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
            else
            {
                ViewBag.ProfilePicturePath = "../../Content/Images/Profile.jpg";
                ViewBag.ProfilePicturePathWithOutSlash = "../../Content/Images/Profile.jpg";
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contacto()
        {
            ViewBag.Message = "Contacto";

            return View();
        }

        [HttpPost]
        public ActionResult Contacto(ContactDTO c)
        {
            if (ModelState.IsValid)
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                MailAddress from = new MailAddress(c.Email);
                StringBuilder sb = new StringBuilder();
                msg.From = from;
                msg.To.Add("info@escueladelaciudad.com.ar");

                if (c.SendMeCopy)
                {
                    msg.To.Add(from);
                }

                msg.Subject = "Nueva consulta WEB";

                if (!string.IsNullOrEmpty(c.Subject))
                {
                    msg.Subject += ": " + c.Subject;
                }

                msg.IsBodyHtml = false;
                smtp.Host = "mail.escueladelaciudad.com.ar";
                smtp.Port = 25;
                sb.Append("Nombre: " + c.Name);
                sb.Append(Environment.NewLine);
                sb.Append("Email: " + c.Email);
                sb.Append(Environment.NewLine);
                sb.Append("Comentario: " + c.Comment);
                msg.Body = sb.ToString();
                smtp.Send(msg);
                msg.Dispose();

                return View("MessageSent");
            }

            return View();
        }

        public ActionResult MessageSent()
        {
            ViewBag.Message = "Enviado!";

            return View();
        }

        public ActionResult LaCasa()
        {
            ViewBag.Message = "La Casa";

            return View();
        }

        public ActionResult Propuesta()
        {
            ViewBag.Message = "Propuesta";

            return View();
        }

        public ActionResult Aula()
        {
            ViewBag.Message = "En el aula";

            return View();
        }

        public ActionResult Institucional()
        {
            ViewBag.Message = "Institucional";

            return View();
        }

        public ActionResult Biblioteca()
        {
            ViewBag.Message = "Biblioteca";

            return View();
        }
    }
}