using PontoRemoto.Application.Interfaces.Infrastructure.Data;
using System.Data.Entity;
using System.Linq;

namespace PontoRemoto.Infra.Services.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _easyDbContext;

        public GenericRepository(ApplicationDbContext easyDbContext)
        {
            this._easyDbContext = easyDbContext;
        }

        public IQueryable<T> Query()
        {
            return this._easyDbContext.Set<T>();
        }

        public virtual T Get(object id)
        {
            return this._easyDbContext.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            this._easyDbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this._easyDbContext.Set<T>().Attach(entity);
            this._easyDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            var entity = this._easyDbContext.Set<T>().Find(id);

            this.Delete(entity);
        }

        public void Delete(T entity)
        {
            if (this._easyDbContext.Entry(entity).State == EntityState.Detached)
            {
                this._easyDbContext.Set<T>().Attach(entity);
            }

            this._easyDbContext.Set<T>().Remove(entity);
        }
    }
}