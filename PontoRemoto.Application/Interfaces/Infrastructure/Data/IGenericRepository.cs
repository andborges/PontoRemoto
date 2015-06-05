using System.Linq;

namespace PontoRemoto.Application.Interfaces.Infrastructure.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();

        T Get(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(object id);

        void Delete(T entity);
    }
}