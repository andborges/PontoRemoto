using Owin;

namespace PontoRemoto.Application.Interfaces.Auth
{
    public interface IIdentitySetup
    {
        void Configure(IAppBuilder app);
    }
}