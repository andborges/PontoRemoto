using PontoRemoto.Application.Resources;
using PontoRemoto.Web.Models;
using System.Web.Mvc;

namespace PontoRemoto.Web.Controllers
{
    public class ClientController : BaseController
    {
        public JsonResult UniqueIdentification(string identification)
        {
            return Json(this.ClientService.Client(identification) == null, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "ClientOwner")]
        public ActionResult Configuration()
        {
            var model = new ClientConfigurationViewModel(this.LoggedClient);

            return View(model);
        }

        [Authorize(Roles = "ClientOwner")]
        [HttpPost]
        public ActionResult Configuration(ClientConfigurationViewModel model)
        {
            var client = this.LoggedClient;
            client.WorkerIdentificationLabel = model.WorkerIdentificationLabel;
            client.UrlCheckinNotification = model.UrlCheckinNotification;

            this.ClientService.Update(client);
            this.StatusMessage(Messages.ClientConfigurationUpdated, MessageTypes.Success);

            model.AppCode = client.AppCode;
            model.AppSecret = client.AppSecret;

            return View(model);
        }
    }
}