using Ninject;
using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Interfaces.Auth;
using PontoRemoto.Application.Interfaces.Business;
using PontoRemoto.Application.Interfaces.Infrastructure.Data;
using PontoRemoto.Application.Interfaces.Infrastructure.System;
using PontoRemoto.Application.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace PontoRemoto.Application.Services.Business
{
    public class ClientService : IClientService
    {
        [Inject]
        public IUnitOfWork UnitOfWork { get; set; }

        [Inject]
        public IApplicationUserManager UserManager { get; set; }

        [Inject]
        public IHttpService HttpService { get; set; }

        public IEnumerable<Client> Clients()
        {
            return this.UnitOfWork.Repository<Client>().Query();
        }

        public Client Client(int id)
        {
            return this.UnitOfWork.Repository<Client>().Get(id);
        }

        public Client Client(ApplicationUser user)
        {
            return this.UnitOfWork.Repository<Client>().Query().ByUserId(user.Id);
        }

        public Client Client(string identification)
        {
            return this.UnitOfWork.Repository<Client>().Query().ByIdentification(identification);
        }

        public Client Client(string appCode, string appSecret)
        {
            return this.UnitOfWork.Repository<Client>().Query().ByAppCredentials(appCode, appSecret);
        }

        public ServiceResult<Client> Create(Client client, ApplicationUser user, string password)
        {
            var errors = new List<string>();

            errors.AddRange(this.ValidateClient(client));
            errors.AddRange(this.ValidateUser(user));

            if (errors.Any())
            {
                return ServiceResult<Client>.Error(errors);
            }

            var existingClient = this.UnitOfWork.Repository<Client>().Query().ByEmail(user.UserName);

            // User can already exist, but with another role
            if (existingClient != null)
            {
                user = this.UnitOfWork.Repository<ApplicationUser>().Query().ByEmail(user.UserName);
            }
            else
            {
                errors.AddRange(this.CreateUser(user, password));
            }

            if (errors.Any())
            {
                return ServiceResult<Client>.Error(errors);
            }

            errors.AddRange(this.AddUserToClientOwnerRole(user));

            if (errors.Any())
            {
                return ServiceResult<Client>.Error(errors);
            }

            client.UserId = user.Id;

            this.UnitOfWork.Repository<Client>().Add(client);
            this.UnitOfWork.SaveChanges();

            return ServiceResult<Client>.Success(client);
        }

        public ServiceResult<Client> Update(Client client)
        {
            this.UnitOfWork.SaveChanges();

            return ServiceResult<Client>.Success(client);
        }

        public ServiceResult<Client> Delete(Client client)
        {
            this.UnitOfWork.Repository<Client>().Delete(client);
            this.UnitOfWork.SaveChanges();

            return ServiceResult<Client>.Success(client);
        }

        public ServiceResult<Checkin> Notify(Client client, Checkin checkin)
        {
            var values = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string, string>("RequestId", Guid.NewGuid().ToString()),
                                 new KeyValuePair<string, string>("Hash", checkin.Hash),
                                 new KeyValuePair<string, string>("AppCode", client.AppCode),
                                 new KeyValuePair<string, string>("AppSecret", client.AppSecret),
                                 new KeyValuePair<string, string>("Identification", checkin.Worker.Identification),
                                 new KeyValuePair<string, string>("Type", checkin.Type.ToString()),
                                 new KeyValuePair<string, string>("Date", checkin.Date.ToString("dd/MM/yyyy hh:mm:ss")),
                                 new KeyValuePair<string, string>("Latitude", checkin.Latitude.ToString(CultureInfo.InvariantCulture)),
                                 new KeyValuePair<string, string>("Longitude", checkin.Longitude.ToString(CultureInfo.InvariantCulture))
                             };

            var httpResult = HttpService.Post(client.UrlCheckinNotification, values);

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                return ServiceResult<Checkin>.Success(checkin);
            }

            return ServiceResult<Checkin>.Error(new List<string> { string.Format("Invalid HttpStatusCode: {0}", httpResult.StatusCode) });
        }

        private IEnumerable<string> ValidateClient(Client newClient)
        {
            var errors = new List<string>();

            if (this.UnitOfWork.Repository<Client>().Query().ByIdentification(newClient.Identification) != null)
            {
                errors.Add(Messages.ClientIdentificationAlreadyExists);
            }

            return errors;
        }

        private IEnumerable<string> ValidateUser(ApplicationUser newUser)
        {
            var errors = new List<string>();

            var existingUser = this.UnitOfWork.Repository<ApplicationUser>().Query().ByEmail(newUser.UserName);

            if (existingUser != null)
            {
                if (this.UserManager.GetUserRoles(existingUser.Id).Contains("ClientOwner"))
                {
                    errors.Add(Messages.ClientUsernameAlreadyExists);
                }
            }

            return errors;
        }

        private IEnumerable<string> CreateUser(ApplicationUser user, string password)
        {
            var errors = new List<string>();

            var userResult = this.UserManager.CreateUser(user, password);

            if (!userResult.Succeeded)
            {
                errors.AddRange(userResult.Errors);
            }

            return errors;
        }

        private IEnumerable<string> AddUserToClientOwnerRole(ApplicationUser user)
        {
            var errors = new List<string>();
            var roleResult = this.UserManager.AddUserToRole(user.Id, "ClientOwner");

            if (!roleResult.Succeeded)
            {
                errors.AddRange(roleResult.Errors);
            }

            return errors;
        }
    }

    public static class ClientQueryExtensions
    {
        public static Client ByUserId(this IQueryable<Client> query, string userId)
        {
            return query.FirstOrDefault(c => c.UserId == userId);
        }

        public static Client ByIdentification(this IQueryable<Client> query, string identification)
        {
            return query.FirstOrDefault(c => c.Identification == identification);
        }

        public static Client ByAppCredentials(this IQueryable<Client> query, string appCode, string appSecret)
        {
            return query.FirstOrDefault(c => c.AppCode == appCode && c.AppSecret == appSecret);
        }

        public static Client ByEmail(this IQueryable<Client> query, string email)
        {
            return query.FirstOrDefault(c => c.User.Email == email);
        }
    }

    public static class ApplicationUserQueryExtensions
    {
        public static ApplicationUser ByEmail(this IQueryable<ApplicationUser> query, string email)
        {
            return query.FirstOrDefault(c => c.Email == email);
        }
    }

    public static class WorkerQueryExtensions
    {
        public static Worker ById(this IQueryable<Worker> query, int clientId, long id)
        {
            return query.FirstOrDefault(w => w.ClientId == clientId && w.Id == id);
        }

        public static IQueryable<Worker> ByClientId(this IQueryable<Worker> query, int clientId)
        {
            return query.Where(w => w.ClientId == clientId);
        }

        public static Worker ByDeviceId(this IQueryable<Worker> query, int clientId, string deviceId)
        {
            return query.FirstOrDefault(w => w.ClientId == clientId && w.DeviceId == deviceId);
        }
    }
}