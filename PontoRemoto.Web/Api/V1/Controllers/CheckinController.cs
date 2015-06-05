using Ninject;
using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Encryption;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Web.Api.V1.Models;
using PontoRemoto.Web.Helpers;
using System.Web.Http;

namespace PontoRemoto.Web.Api.V1.Controllers
{
    [ValidateHash(typeof(ApplicationAesEncryptionInfo))]
    public class CheckinController : ApiController
    {
        [Inject]
        public IWorkerService WorkerService { get; set; }

        [Route("Api/V1/Checkin")]
        public IHttpActionResult Post(CreateCheckinViewModel model)
        {
            if (ModelState.IsValid)
            {
                var worker = WorkerService.Worker(model.ClientId, model.DeviceId);

                if (worker == null || worker.Status != WorkerStatus.Granted)
                {
                    return NotFound();
                }

                var checkin = new Checkin
                {
                    Type = model.Type,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };

                var result = WorkerService.Checkin(worker, checkin);

                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest(string.Join(" ", result.Errors));
            }

            return BadRequest(ModelState);
        }
    }
}