using Ninject;
using PontoRemoto.Application.Encryption;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Web.Api.V1.Models;
using PontoRemoto.Web.Helpers;
using System.Web.Http;

namespace PontoRemoto.Web.Api.V1.Controllers
{
    [ValidateHash(typeof(ApplicationAesEncryptionInfo))]
    public class ClientController : ApiController
    {
        [Inject]
        public IClientService ClientService { get; set; }

        [Route("Api/V1/Client")]
        public IHttpActionResult Get([FromUri] GetClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = ClientService.Client(model.Identification);

                if (client == null)
                {
                    return NotFound();
                }

                return Ok(new ClientViewModel(client));
            }

            return BadRequest(ModelState);
        }
    }
}