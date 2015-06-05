using PontoRemoto.Application.Domain;
using System.Collections.Generic;

namespace PontoRemoto.Application.Interfaces.Business
{
    public interface IClientService
    {
        IEnumerable<Client> Clients();

        Client Client(int id);

        Client Client(ApplicationUser user);

        Client Client(string identification);

        Client Client(string appCode, string appSecret);

        ServiceResult<Client> Create(Client client, ApplicationUser user, string password);

        ServiceResult<Client> Update(Client client);

        ServiceResult<Client> Delete(Client client);

        ServiceResult<Checkin> Notify(Client client, Checkin checkin);
    }
}