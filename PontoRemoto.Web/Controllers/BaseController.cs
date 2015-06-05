using Ninject;
using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Interfaces.Auth;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Web.Models;
using PontoRemoto.Web.Services;
using System.Web.Mvc;

namespace PontoRemoto.Web.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public ISessionService SessionService { get; set; }

        [Inject]
        public IClientService ClientService { get; set; }

        [Inject]
        public IApplicationUserManager UserManager { get; set; }

        public ApplicationUser LoggedUser
        {
            get
            {
                var loggedUser = this.SessionService.Get("LoggedUser") as ApplicationUser;

                if (Request.IsAuthenticated && loggedUser == null)
                {
                    loggedUser = this.UserManager.FindByUserName(User.Identity.Name);
                    this.SessionService.Add("LoggedUser", loggedUser);
                }

                return loggedUser;
            }
        }

        public Client LoggedClient
        {
            get
            {
                return this.ClientService.Client(LoggedUser);
            }
        }

        public void StatusMessage(string message, MessageTypes messageType)
        {
            TempData["StatusMessageText"] = message;
            TempData["StatusMessageType"] = messageType;
        }
    }
}