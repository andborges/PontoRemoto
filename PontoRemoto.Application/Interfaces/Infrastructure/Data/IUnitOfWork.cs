namespace PontoRemoto.Application.Interfaces.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;

        int SaveChanges();
    }
}