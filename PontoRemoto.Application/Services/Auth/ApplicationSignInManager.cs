using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Interfaces.Auth;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PontoRemoto.Application.Services.Auth
{
    [ExcludeFromCodeCoverage]
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>, IApplicationSignInManager
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
}
