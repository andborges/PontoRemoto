using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace PontoRemoto.Application.Interfaces.Infrastructure.System
{
    public interface IEmailService : IIdentityMessageService
    {
        Task SendAsync(string to, string subject, string message);
    }
}