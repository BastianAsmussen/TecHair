using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace API.Utility.Database.DAL;

public sealed class GenericRepository<T>(DbContext context)
    where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>?> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return await (orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
    }


    public async Task<T?> GetById(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async void Insert(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }


    public async void Delete(object id)
    {
        var entity = await _dbSet.FindAsync(id);

        Delete(entity);
    }

    public void Delete(T entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }
}
