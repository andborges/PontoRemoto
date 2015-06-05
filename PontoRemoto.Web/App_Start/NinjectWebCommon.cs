using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;

using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using PontoRemoto.Web.Services;
using System;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PontoRemoto.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PontoRemoto.Web.App_Start.NinjectWebCommon), "Stop")]

namespace PontoRemoto.Web.App_Start
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            DependencyResolution.Config.RegisterServices(kernel);

            kernel.Bind<ISessionService>().To<SessionService>();
        }        
    }

    public class NinjectResolver : NinjectScope, IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectResolver(IKernel kernel)
            : base(kernel)
        {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectScope(_kernel.BeginBlock());
        }
    }

    public class NinjectScope : IDependencyScope
    {
        protected IResolutionRoot ResolutionRoot;

        public NinjectScope(IResolutionRoot kernel)
        {
            ResolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            var request = ResolutionRoot.CreateRequest(serviceType, null, new Ninject.Parameters.Parameter[0], true, true);
            return ResolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var request = ResolutionRoot.CreateRequest(serviceType, null, new Ninject.Parameters.Parameter[0], true, true);
            return ResolutionRoot.Resolve(request).ToList();
        }

        public void Dispose()
        {
            var disposable = (IDisposable)ResolutionRoot;
            if (disposable != null) disposable.Dispose();
            ResolutionRoot = null;
        }
    }
}
