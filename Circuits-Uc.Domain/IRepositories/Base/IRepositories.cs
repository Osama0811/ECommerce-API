using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Domain.IRepositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        IQueryable<T> All();
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);

        Task<List<T>> AddRangeAsync(List<T> entities);

        Task<List<T>> DeleteRangeAsync(List<T> entities);


        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> SoftDelete(Guid Id);
        Task<bool> SoftDeleteRangeAsync(List<Guid> entities);

    }
}
