using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PontoRemoto.Application.Services.Auth
{
    [ExcludeFromCodeCoverage]
    public class ApplicationUserManager : UserManager<ApplicationUser>, IApplicationUserManager
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            this.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });

            this.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            var dataProtectionProvider = IdentitySetup.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

        // These methods were necessary for unit tests reasons
        // ApplicationUserManager.FindByName is a extension method and
        // could not be mocked

        public ApplicationUser FindByUserName(string userName)
        {
            return this.FindByName(userName);
        }

        public IEnumerable<string> GetUserRoles(string userId)
        {
            return this.GetRoles(userId);
        }

        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            this.UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            return this.Create(user, password);
        }

        public IdentityResult AddUserToRole(string userId, string role)
        {
            return this.AddToRole(userId, role);
        }
    }
}