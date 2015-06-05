using Ninject;
using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Encryption;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Web.Api.V1.Models;
using System.Web.Http;

using PontoRemoto.Web.Helpers;

namespace PontoRemoto.Web.Api.V1.Controllers
{
    [ValidateHash(typeof(ApplicationAesEncryptionInfo))]
    public class WorkerController : ApiController
    {
        [Inject]
        public IWorkerService WorkerService { get; set; }

        [Route("Api/V1/Worker")]
        public IHttpActionResult Post(CreateWorkerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var worker = new Worker
                {
                    ClientId = model.ClientId,
                    DeviceId = model.DeviceId,
                    DeviceModel = model.DeviceModel,
                    DevicePlatform = model.DevicePlatform,
                    DeviceAlias = model.DeviceAlias,
                    Identification = model.Identification,
                    Name = model.Name,
                    Status = WorkerStatus.New
                };

                var result = WorkerService.Create(worker);

                if (result.Succeeded)
                {
                    return Ok(new WorkerViewModel(worker));
                }

                return BadRequest(string.Join(" ", result.Errors));
            }

            return BadRequest(ModelState);
        }

        [Route("Api/V1/Worker")]
        public IHttpActionResult Get([FromUri] GetWorkerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var worker = WorkerService.Worker(model.ClientId, model.DeviceId);

                if (worker == null)
                {
                    return NotFound();
                }

                return Ok(new WorkerViewModel(worker));
            }

            return BadRequest(ModelState);
        }
    }
}
