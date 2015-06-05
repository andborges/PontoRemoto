using Microsoft.AspNet.Identity;
using PontoRemoto.Application.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PontoRemoto.Application.Interfaces.Auth
{
    public interface IApplicationUserManager : IDisposable
    {
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<ApplicationUser> FindByIdAsync(string userId);

        Task<ApplicationUser> FindByNameAsync(string username);

        ApplicationUser FindByUserName(string userName);

        Task<bool> IsEmailConfirmedAsync(string userId);

        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);

        IEnumerable<string> GetUserRoles(string userId);

        IdentityResult CreateUser(ApplicationUser user, string password);

        IdentityResult AddUserToRole(string userId, string role);

        new void Dispose();
    }
}