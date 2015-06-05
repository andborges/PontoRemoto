using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Interfaces.Auth;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Application.Interfaces.Infrastructure.Data;
using PontoRemoto.Application.Interfaces.Infrastructure.System;
using PontoRemoto.Application.Services.Auth;
using PontoRemoto.Application.Services.Business;
using PontoRemoto.Infra.Services.Data;
using PontoRemoto.Infra.Services.System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Ninject;
using Ninject.Web.Common;
using System.Web;

namespace PontoRemoto.DependencyResolution
{
    public class Config
    {
        public static void RegisterServices(IKernel kernel)
        {
            // Data
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            // Auth
            kernel.Bind<IIdentitySetup>().To<IdentitySetup>();
            kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>().WithConstructorArgument("context", kernel.Get<ApplicationDbContext>());
            kernel.Bind<IAuthenticationManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Authentication);
            kernel.Bind<IApplicationUserManager>().To<ApplicationUserManager>();
            kernel.Bind<IApplicationSignInManager>().To<ApplicationSignInManager>();

            // System
            kernel.Bind<IHttpService>().To<HttpService>();
            kernel.Bind<IEmailService>().To<EmailService>();

            // Business
            kernel.Bind<IClientService>().To<ClientService>();
            kernel.Bind<IWorkerService>().To<WorkerService>();
        }
    }
}
