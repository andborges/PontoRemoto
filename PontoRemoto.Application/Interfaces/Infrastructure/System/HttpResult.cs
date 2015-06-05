using System.Net;

namespace PontoRemoto.Application.Interfaces.Infrastructure.System
{
    public class HttpResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Content { get; set; }
    }
}