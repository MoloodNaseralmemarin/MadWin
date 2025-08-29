using MadWin.Core.Entities.Common;

namespace MadWin.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        void Remove(T entity);
        void Update(T entity);
        Task SaveChangesAsync();

        IQueryable<T> GetQuery();

    }
}
