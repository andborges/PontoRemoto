using Ninject;
using PontoRemoto.Application.Interfaces.Infrastructure.Data;
using System;

namespace PontoRemoto.Infra.Services.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        [Inject]
        public ApplicationDbContext ApplicationDbContext { get; set; }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            return new GenericRepository<T>(this.ApplicationDbContext);
        }

        public int SaveChanges()
        {
            return this.ApplicationDbContext.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this.ApplicationDbContext.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}