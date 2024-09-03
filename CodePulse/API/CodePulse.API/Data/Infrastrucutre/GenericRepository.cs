using System.Linq.Expressions;
using CodePulse.API.Data.EFs;
using CodePulse.API.Data.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data.Infrastrucutre;

public class GenericRepository<T> : IGenericReposiory<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task AddAsync(T item)
    {
        await _dbContext.Set<T>().AddAsync(item);
    }

    public async Task<T?> GetByIdAsync(Guid Id)
    {
        var item = await _dbContext.Set<T>().FindAsync(Id);
        return item;
    }

    public void Update(T item)
    {
        _dbContext.Set<T>().Update(item);
    }

    public async Task<T?> DeleteAsync(Guid Id)
    {
        var item = await _dbContext.Set<T>().FindAsync(Id);
        if (item is null) return null;
        _dbContext.Set<T>().Remove(item);
        return item;
    }

    public async Task<IEnumerable<T>> GetAllUpdatedAsync(Func<IQueryable<T>, IQueryable<T>>? include,
        Expression<Func<T, bool>>? predicate)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdUpdatedAsync(Func<IQueryable<T>, IQueryable<T>>? include,
        Expression<Func<T, bool>>? predicate)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        string? sortBy = null,
        string? sortDirection = null, 
        int? page = 1,
        int? pageSize = 100)
    {
        // Query
        var queryable = _dbContext.Set<T>().AsQueryable();
        //Filtering
        if (predicate is not null) queryable = queryable.Where(predicate);
        // Sorting
        if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
        {
            var param = Expression.Parameter(typeof(T), "item");
            var sortExpression = Expression.Lambda<Func<T, object>>(
                Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            if (sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase))
                queryable = queryable.OrderBy(sortExpression);
            else
                queryable = queryable.OrderByDescending(sortExpression);
        }
        //Pagination
        var skip = pageSize * (page - 1);
        queryable = queryable.Skip(skip??0).Take(pageSize??100);
         
        return await queryable.ToListAsync();
    }
}