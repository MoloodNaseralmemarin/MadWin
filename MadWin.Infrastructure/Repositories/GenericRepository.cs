
using MadWin.Core.Entities.Common;
using MadWin.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public void Remove(T entity) => _dbSet.Remove(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public IQueryable<T> GetQuery()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<List<T>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _context.Set<T>()
            .Where(x => ids.Contains(x.Id) && !x.IsDelete)
            .ToListAsync();
    }

    public async Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>()
            .Where(predicate)
            .ToListAsync();
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _context.Set<T>().UpdateRange(entities);
    }


}

