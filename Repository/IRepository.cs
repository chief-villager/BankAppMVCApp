using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BankApp.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(int Id);
        Task<T?> GetByEmailAsync(Expression<Func<T,bool>> predicate );
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(int Id, T t, CancellationToken cancellationToken);
        Task DeleteAsync(int Id, CancellationToken cancellationToken);
    }

}