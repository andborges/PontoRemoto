using PontoRemoto.Application.Encryption;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PontoRemoto.Web.Helpers
{
    public class ValidateHashAttribute : ActionFilterAttribute
    {
        private Type AesEncryptionInfoType { get; set; }

        public ValidateHashAttribute(Type aesEncryptionInfoType)
        {
            this.AesEncryptionInfoType = aesEncryptionInfoType;
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var hashPair = context.Request.GetQueryNameValuePairs().FirstOrDefault(q => q.Key == "hash");

            if (hashPair.Value == null)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }

            try
            {
                var aesEncryptionInfo = (ApplicationAesEncryptionInfo)Activator.CreateInstance(this.AesEncryptionInfoType);

                var hash = hashPair.Value.Replace(" ", "+");
                var decryptedInfo = hash.AesDecrypt(aesEncryptionInfo);

                Guid guid;

                if (!Guid.TryParse(decryptedInfo, out guid))
                {
                    context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                }
            }
            catch (Exception)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}