using DataBaseModels;
using EnumClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessManager
{
    public interface IEFCoreDataAccessManager<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(int Id, T entity);
        Task<T> DeleteAsync(int Id, T entity);
        Task SaveChangesAsync();
        Task DisposeAsync();
        Task Transition();
        Task CommitAsync();
        Task RollbackAsync();
    }
    public class EFCoreDataAccessManager<T> : IEFCoreDataAccessManager<T> where T : class
    {
        private readonly AppDBConnection _dbContext;
        private readonly DbSet<T> _dbSet;
        private IDbContextTransaction? _transaction;
        public EFCoreDataAccessManager(AppDBConnection dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task Transition()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var result = await _dbSet.FindAsync(id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<T> InsertAsync(T entity)
        {
            var re = await _dbSet.AddAsync(entity);
            return entity;
        }
        public async Task<T> UpdateAsync(int Id, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Empty");
            }

            var data = await _dbSet.FindAsync(Id);
            if (data != null)
            {
                _dbSet.Entry(data).CurrentValues.SetValues(entity);
            }
            return entity;
        }

        public async Task<T> DeleteAsync(int Id,T entity)
        {
            var data = await _dbSet.FindAsync(Id);

            if (data != null)
            {
                data = entity;
                _dbSet.Entry(data).CurrentValues.SetValues(entity);
                return entity;
            }
            return null;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
    }
}
