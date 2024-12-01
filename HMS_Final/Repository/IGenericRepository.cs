using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace HMS_Final.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        public IQueryable<T> GetDbSet();

    }
}
