using Ninject;
using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Encryption;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Application.Interfaces.Infrastructure.Data;
using PontoRemoto.Application.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace PontoRemoto.Application.Services.Business
{
    public class WorkerService : IWorkerService
    {
        [Inject]
        public IUnitOfWork UnitOfWork { get; set; }

        [Inject]
        public IClientService ClientService { get; set; }

        public IEnumerable<Worker> Workers(int clientId)
        {
            return this.UnitOfWork.Repository<Worker>().Query().ByClientId(clientId);
        }

        public Worker Worker(int clientId, string deviceId)
        {
            return this.UnitOfWork.Repository<Worker>().Query().ByDeviceId(clientId, deviceId);
        }

        public Worker Worker(int clientId, long id)
        {
            return this.UnitOfWork.Repository<Worker>().Query().ById(clientId, id);
        }

        public ServiceResult<Worker> Create(Worker worker)
        {
            var errors = new List<string>();

            errors.AddRange(this.ValidateWorker(worker));

            if (errors.Any())
            {
                return ServiceResult<Worker>.Error(errors);
            }

            this.UnitOfWork.Repository<Worker>().Add(worker);
            this.UnitOfWork.SaveChanges();

            return ServiceResult<Worker>.Success(worker);
        }

        public ServiceResult<Worker> Update(Worker worker)
        {
            this.UnitOfWork.SaveChanges();

            return ServiceResult<Worker>.Success(worker);
        }

        public ServiceResult<Checkin> Checkin(Worker worker, Checkin checkin)
        {
            checkin.WorkerId = worker.Id;
            checkin.Worker = worker;
            checkin.Hash = new JavaScriptSerializer().Serialize(checkin).AesEncrypt(new ApplicationAesEncryptionInfo());

            this.UnitOfWork.Repository<Checkin>().Add(checkin);
            this.UnitOfWork.SaveChanges();

            var result = ClientService.Notify(worker.Client, checkin);

            if (result.Succeeded)
            {
                checkin.Notified = true;
                this.UnitOfWork.SaveChanges();
            }

            return ServiceResult<Checkin>.Success(checkin);
        }

        public ServiceResult<Worker> GrantAccess(int clientId, long id)
        {
            var worker = this.UnitOfWork.Repository<Worker>().Query().ById(clientId, id);

            if (worker == null)
            {
                throw new ArgumentException(Messages.WorkerNotFound);
            }

            worker.Status = WorkerStatus.Granted;
            
            this.UnitOfWork.SaveChanges();

            return ServiceResult<Worker>.Success(worker);
        }

        public ServiceResult<Worker> RevokeAccess(int clientId, long id)
        {
            var worker = this.UnitOfWork.Repository<Worker>().Query().ById(clientId, id);

            if (worker == null)
            {
                throw new ArgumentException(Messages.WorkerNotFound);
            }

            worker.Status = WorkerStatus.Revoked;

            this.UnitOfWork.SaveChanges();

            return ServiceResult<Worker>.Success(worker);
        }

        private IEnumerable<string> ValidateWorker(Worker worker)
        {
            var errors = new List<string>();

            if (this.UnitOfWork.Repository<Worker>().Query().ByDeviceId(worker.ClientId, worker.DeviceId) != null)
            {
                errors.Add(Messages.WorkerDeviceIdAlreadyExists);
            }

            return errors;
        }
    }
}