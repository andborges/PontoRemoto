using Microsoft.AspNet.Identity.EntityFramework;
using PontoRemoto.Application.Domain;
using PontoRemoto.Infra.Services.Data;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PontoRemoto.Infra.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Set<IdentityRole>().AddOrUpdate(
                r => r.Name,
                new[]
                {
                        new IdentityRole { Name = "SystemAdministrator" },
                        new IdentityRole { Name = "ClientOwner" }
                }
            );

            context.Set<ApplicationUser>().AddOrUpdate(
                r => r.UserName,
                new[]
                {
                    new ApplicationUser
                    {
                        Name = "Administrator",
                        UserName = "admin",
                        PasswordHash = "AKFcDNoNPfcsSXe5ADKgQOqhDP/hyLKLc7oT/mcQ1S+98AaHD+l9KGLB8jLWQiN+Iw==",
                        SecurityStamp = "3b6636a8-341e-4952-9bf2-816cc62ba9c0"
                    },
                    new ApplicationUser
                    {
                        Name = "Test Client 1",
                        UserName = "client1@test.com",
                        PasswordHash = "AIPPcWlKwyekcr3YsecbDQY4MBGIuCA89ALxPcySk0toWqJdWOXjwIF1PBWgQKHw1Q==",
                        SecurityStamp = "8dbd6f60-ca2d-48e6-ac3d-c910e2679028"
                    },
                    new ApplicationUser
                    {
                        Name = "Test Client 2",
                        UserName = "client2@test.com",
                        PasswordHash = "AIPPcWlKwyekcr3YsecbDQY4MBGIuCA89ALxPcySk0toWqJdWOXjwIF1PBWgQKHw1Q==",
                        SecurityStamp = "8dbd6f60-ca2d-48e6-ac3d-c910e2679028"
                    }
                }
            );

            context.Set<IdentityUserRole>().AddOrUpdate(
                r => new { r.UserId, r.RoleId },
                new[]
                {
                    new IdentityUserRole
                    {
                        UserId = context.Set<ApplicationUser>().Local.First(u => u.UserName == "admin").Id,
                        RoleId = context.Set<IdentityRole>().Local.First(u => u.Name == "SystemAdministrator").Id
                    },
                    new IdentityUserRole
                    {
                        UserId = context.Set<ApplicationUser>().Local.First(u => u.UserName == "client1@test.com").Id,
                        RoleId = context.Set<IdentityRole>().Local.First(u => u.Name == "ClientOwner").Id
                    },
                    new IdentityUserRole
                    {
                        UserId = context.Set<ApplicationUser>().Local.First(u => u.UserName == "client2@test.com").Id,
                        RoleId = context.Set<IdentityRole>().Local.First(u => u.Name == "ClientOwner").Id
                    }
                }
            );

            context.Set<Client>().AddOrUpdate(
                r => new { r.Name },
                new[]
                {
                    new Client
                    {
                        Name = "Test Client 1",
                        Identification = "TestClient1",
                        UserId = context.Set<ApplicationUser>().Local.First(u => u.UserName == "client1@test.com").Id,
                        WorkerIdentificationLabel = "CPF"
                    },
                    new Client
                    {
                        Name = "Test Client 2",
                        Identification = "TestClient2",
                        UserId = context.Set<ApplicationUser>().Local.First(u => u.UserName == "client2@test.com").Id,
                        WorkerIdentificationLabel = "Matrícula"
                    }
                }
            );
        }
    }
}
