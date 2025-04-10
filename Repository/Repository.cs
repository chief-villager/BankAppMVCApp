using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;


namespace BankApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Microsoft.EntityFrameworkCore.DbSet<T> _dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync();
            return entity;

        }

        public async Task DeleteAsync(int Id, CancellationToken cancellationToken)
        {
            T? t = Id <= 0 ? throw new ArgumentOutOfRangeException(nameof(Id)) : await _dbSet.FindAsync(Id, cancellationToken);
            _ = t ?? throw new ArgumentNullException(nameof(t));
            _dbSet.Remove(t);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<T?> GetByEmailAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            T? t = Id <= 0 ? throw new ArgumentOutOfRangeException(nameof(Id)) : await _dbSet.FindAsync(Id);
            return t;
        }
        public async Task UpdateAsync(int Id, T t, CancellationToken cancellationToken)
        {
            T? existingT = Id <= 0 ? throw new ArgumentOutOfRangeException(nameof(Id)) : await _dbSet.FindAsync(Id, cancellationToken);
            _ = existingT ?? throw new ArgumentNullException(nameof(existingT));
            _dbSet.Entry(existingT).CurrentValues.SetValues(t);
            await _dbContext.SaveChangesAsync(cancellationToken);

        }
    }
}