using Microsoft.AspNet.Identity;
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
    public class ManageController : BaseController
    {
        [Inject]
        public IApplicationSignInManager SignInManager { get; set; }

        [Inject]
        public IAuthenticationManager AuthenticationManager { get; set; }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();

            var result = await this.UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var user = await this.UserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                this.StatusMessage(Messages.PasswordChangedSuccess, MessageTypes.Success);

                return RedirectToAction("Index", "Home");
            }
            
            AddErrors(result);
            
            return View(model);
        }

        [ExcludeFromCodeCoverage]
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.UserManager != null)
            {
                this.UserManager.Dispose();
                this.UserManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError("", error);
            }
        }

#endregion
    }
}