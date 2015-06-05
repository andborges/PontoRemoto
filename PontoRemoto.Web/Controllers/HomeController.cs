using System.Configuration;

using Ninject;
using PontoRemoto.Application.Interfaces.Infrastructure.System;
using PontoRemoto.Web.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoRemoto.Web.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public IEmailService EmailService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Contact(string company, string name, string email)
        {
            var message = string.Format("Empresa: {0}<br/>Seu nome: {1}<br/>Seu email: {2}", company, name, email);

            await this.EmailService.SendAsync(ConfigurationManager.AppSettings["ContactEmail"], "Contato Ponto Remoto", message);

            return Json(new JsonViewModel(true, "", null));
        }

        [Authorize]
        public ActionResult Admin()
        {
            return View();
        }
    }
}