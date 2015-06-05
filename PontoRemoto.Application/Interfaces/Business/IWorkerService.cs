using PontoRemoto.Application.Domain;
using System.Collections.Generic;

namespace PontoRemoto.Application.Interfaces.Business
{
    public interface IWorkerService
    {
        IEnumerable<Worker> Workers(int clientId);

        Worker Worker(int clientId, string deviceId);

        Worker Worker(int clientId, long id);

        ServiceResult<Worker> Create(Worker worker);

        ServiceResult<Worker> Update(Worker worker);

        ServiceResult<Checkin> Checkin(Worker worker, Checkin checkin);

        ServiceResult<Worker> GrantAccess(int clientId, long id);

        ServiceResult<Worker> RevokeAccess(int clientId, long id);
    }
}