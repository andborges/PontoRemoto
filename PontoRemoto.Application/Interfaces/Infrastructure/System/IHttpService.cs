using System.Collections.Generic;
using System.Collections.Specialized;

namespace PontoRemoto.Application.Interfaces.Infrastructure.System
{
    public interface IHttpService
    {
        HttpResult Post(string url, NameValueCollection data);

        HttpResult Post(string url, IEnumerable<KeyValuePair<string, string>> data);
    }
}