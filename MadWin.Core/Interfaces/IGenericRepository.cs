using MadWin.Core.Entities.Common;
using System.Linq.Expressions;

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

        Task<List<T>> GetByIdsAsync(IEnumerable<int> ids);
        Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
        void UpdateRange(IEnumerable<T> entities);

    }
}
