using Ninject;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Application.Resources;
using PontoRemoto.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace PontoRemoto.Web.Controllers
{
    public class WorkerController : BaseController
    {
        [Inject]
        public IWorkerService WorkerService { get; set; }

        [Authorize(Roles = "ClientOwner")]
        public ActionResult Index()
        {
            var client = LoggedClient;

            var model = WorkerService.Workers(client.Id).Select(w => new WorkerViewModel(w));

            return View(model);
        }

        [Authorize(Roles = "ClientOwner")]
        [HttpPost]
        public JsonResult GrantAccess(long id)
        {
            var client = LoggedClient;
            
            var grantAccessResult = WorkerService.GrantAccess(client.Id, id);

            if (grantAccessResult.Succeeded)
            {
                return Json(new JsonViewModel(true, Messages.GrantAccessSuccess, new { Worker = new WorkerViewModel(grantAccessResult.Result) }));
            }

            return Json(new JsonViewModel(false, null, null));
        }

        [Authorize(Roles = "ClientOwner")]
        [HttpPost]
        public JsonResult RevokeAccess(long id)
        {
            var client = LoggedClient;

            var revokeAccessResult = WorkerService.RevokeAccess(client.Id, id);

            if (revokeAccessResult.Succeeded)
            {
                return Json(new JsonViewModel(true, Messages.RevokeAccessSuccess, new { Worker = new WorkerViewModel(revokeAccessResult.Result) }));
            }

            return Json(new JsonViewModel(false, null, null));
        }
    }
}