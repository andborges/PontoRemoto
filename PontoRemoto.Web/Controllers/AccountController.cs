using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ninject;
using PontoRemoto.Application.Interfaces.Auth;
using PontoRemoto.Application.Resources;
using PontoRemoto.Web.Models;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoRemoto.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [Inject]
        public IApplicationSignInManager SignInManager { get; set; }

        [Inject]
        public IAuthenticationManager AuthenticationManager { get; set; }

        [AllowAnonymous]
        public PartialViewResult LoggedUserName()
        {
            var loggedUser = this.LoggedUser;

            return PartialView("_LoggedUserName", loggedUser != null ? loggedUser.Name : string.Empty);
        }

        [AllowAnonymous]
        public PartialViewResult LoggedUserEmail()
        {
            var loggedUser = this.LoggedUser;

            return PartialView("_LoggedUserEmail", loggedUser != null ? loggedUser.UserName : string.Empty);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await this.SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var user = await this.UserManager.FindByNameAsync(model.UserName);
                    this.SessionService.Add("LoggedUser", user);

                    return this.RedirectToLocal(returnUrl);

                default:
                    ModelState.AddModelError("", Messages.LoginInvalid);
                    return View(model);
            }
        }

        /*
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Name = "Nome", UserName = model.Email, Email = model.Email };

                var result = await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    
                    //// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    //// Send an email with this link
                    //// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //// await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        */

        /*
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        */

        /*
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        */

        /*
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);

            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        */

        // POST: /Account/LogOff
        [HttpGet]
        public ActionResult LogOff()
        {
            this.SessionService.Remove("LoggedUser");
            this.AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [ExcludeFromCodeCoverage]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.UserManager != null)
                {
                    this.UserManager.Dispose();
                    this.UserManager = null;
                }

                if (this.SignInManager != null)
                {
                    this.SignInManager.Dispose();
                    this.SignInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        /*
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        */

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}