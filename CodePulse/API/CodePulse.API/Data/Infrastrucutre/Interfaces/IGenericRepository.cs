using System.Linq.Expressions;

namespace CodePulse.API.Data.Infrastructure.Interfaces;

public interface IGenericReposiory<T> where T : class
{
    Task AddAsync(T item);
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        string? sortBy = null,
        string? sortDirection = null,
        int? page = 1, int? pageSize = 10);
    Task<T?> GetByIdAsync(Guid Id);
    void Update(T item);
    Task<T?> DeleteAsync(Guid Id);

    Task<IEnumerable<T>> GetAllUpdatedAsync(Func<IQueryable<T>, IQueryable<T>>? include,
        Expression<Func<T, bool>>? predicate);

    Task<T?> GetByIdUpdatedAsync(Func<IQueryable<T>, IQueryable<T>>? include,
        Expression<Func<T, bool>>? predicate);
}