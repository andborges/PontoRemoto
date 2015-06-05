using Microsoft.AspNet.Identity.EntityFramework;
using PontoRemoto.Application.Domain;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PontoRemoto.Infra.Services.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public IDbSet<Client> Clients { get; set; }

        public IDbSet<Worker> Workers { get; set; }

        public IDbSet<Checkin> Checkins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<ApplicationDbContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}