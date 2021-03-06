﻿using SchoolManager.Domain;
using SchoolManager.Domain.Entities;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication1.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl)
        {
            using (var db = new Context())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("LogOnError", "El usuario o contraseña utilizado no es correcto");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Id + "|" + user.Name, true);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                       && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        //Redirect to default page
                        //return RedirectToAction("Index", "Home");
                        return RedirectToAction("Profile", "User", new { id = user.Id });
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "El usuario o contraseña utilizado no es correcto");
            return View(model);
        }


        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        /*        //
                // GET: /Account/Manage

                public ActionResult Manage(ManageMessageId? message)
                {
                    ViewBag.StatusMessage =
                        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                        : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                        : "";

                    //ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    //ViewBag.ReturnUrl = Url.Action("Manage");

                    return View();
                }

                //
                // POST: /Account/Manage

                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Manage(LocalPasswordModel model)
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    ViewBag.HasLocalPassword = hasLocalAccount;
                    ViewBag.ReturnUrl = Url.Action("Manage");
                    if (hasLocalAccount)
                    {
                        if (ModelState.IsValid)
                        {
                            // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                            bool changePasswordSucceeded;
                            try
                            {
                                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                            }
                            catch (Exception)
                            {
                                changePasswordSucceeded = false;
                            }

                            if (changePasswordSucceeded)
                            {
                                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                            }
                            else
                            {
                                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                            }
                        }
                    }
                    else
                    {
                        // User does not have a local password so remove any validation errors caused by a missing
                        // OldPassword field
                        ModelState state = ModelState["OldPassword"];
                        if (state != null)
                        {
                            state.Errors.Clear();
                        }

                        if (ModelState.IsValid)
                        {
                            try
                            {
                                WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                            }
                            catch (Exception e)
                            {
                                ModelState.AddModelError("", e);
                            }
                        }
                    }

                    // If we got this far, something failed, redisplay form
                    return View(model);
                }

         */

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion Helpers
    }
}