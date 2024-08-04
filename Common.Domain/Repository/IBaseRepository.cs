
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Domain.Repository
{
    public interface IBaseRepository<T, Identifier>
        where Identifier : struct
        where T : AggregateRoot<Identifier>
    {
        Task<Identifier> CreateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(Identifier entityId);
        Task<(int count, Identifier[] identifiers)> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        T? GetById(Identifier identifier);
    }
}
