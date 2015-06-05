using System.Net;

namespace PontoRemoto.Application.Domain
{
    public class HttpResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Content { get; set; }
    }
}