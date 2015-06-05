using PontoRemoto.Application.Domain;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace PontoRemoto.Application.Interfaces.Auth
{
    public interface IApplicationSignInManager
    {
        Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);

        Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser);

        void Dispose();
    }
}