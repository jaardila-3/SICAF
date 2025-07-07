using System.Linq.Expressions;

using SICAF.Data.Entities.Common;

namespace SICAF.Data.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
      Task<IReadOnlyList<T>> GetListAsync();
      Task<IReadOnlyList<T>> GetListAsync(Expression<Func<T, bool>> predicate);
      Task<IReadOnlyList<T>> GetListAsync(
          Expression<Func<T, bool>>? predicate,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
          bool disableTracking = true);

      Task<T?> GetByIdAsync(object id);
      Task<T?> GetFirstAsync(
          Expression<Func<T, bool>>? predicate,
          Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
          bool disableTracking = true);
      Task<T?> GetFirstAsync(
          Expression<Func<T, bool>>? predicate,
          List<string>? includeStrings,
          bool disableTracking = true);

      Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
      Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

      Task AddAsync(T entity);
      Task AddRangeAsync(IEnumerable<T> entities);
      void Update(T entity);
      void Delete(T entity);
      void DeleteRange(IEnumerable<T> entities);

      // Specification pattern
      IQueryable<T> ApplySpecification(ISpecification<T> spec);
      Task<T?> GetFirstBySpecAsync(ISpecification<T> spec);
      Task<IReadOnlyList<T>> GetListWithSpecAsync(ISpecification<T> spec);
      Task<int> CountAsync(ISpecification<T> spec);
}