using PontoRemoto.Application.Interfaces.Infrastructure.System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PontoRemoto.Infra.Services.System
{
    public class HttpService : IHttpService
    {
        public HttpResult Post(string url, NameValueCollection data)
        {
            var result = new HttpResult();

            using (var client = new WebClient())
            {
                try
                {
                    var response = client.UploadValues(url, data);

                    result.StatusCode = HttpStatusCode.OK;
                    result.Content = Encoding.UTF8.GetString(response);
                }
                catch (WebException ex)
                {
                    result.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                }
            }

            return result;
        }

        public HttpResult Post(string url, IEnumerable<KeyValuePair<string, string>> data)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var kvp in data)
            {
                nameValueCollection.Add(kvp.Key, kvp.Value);
            }

            return this.Post(url, nameValueCollection);
        }

        public HttpStatusCode SendJsonRequest(string url, string method, string json)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentLength = json.Length;
            webRequest.ContentType = "application/json";
            webRequest.KeepAlive = false;
            webRequest.Accept = "application/json";
            webRequest.Method = method;
            webRequest.Timeout = 60000;

            ServicePointManager.DnsRefreshTimeout = 0;
            ServicePointManager.ServerCertificateValidationCallback = CertificateHandler;

            using (var requestStream = webRequest.GetRequestStream())
            {
                var requestByteArray = new ASCIIEncoding().GetBytes(json);
                requestStream.Write(requestByteArray, 0, requestByteArray.Length);
                requestStream.Close();
            }

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                return webResponse.StatusCode;
            }
        }

        private static bool CertificateHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}