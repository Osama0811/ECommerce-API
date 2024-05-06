using CircuitsUc.Domain.Entities.Base;
using CircuitsUc.Domain.IRepositories.Base;
using CircuitsUc.InfraStructure.Presistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.InfraStructure.Reposatories.Base
{
    public class Repository<T> : IRepository<T> where T : Auditable
    {
        protected readonly DBContext _context;

        public Repository(DBContext DBDemoContext)
        {
            _context = DBDemoContext;
        }
        

        public IQueryable<T> All()
        {
            return _context.Set<T>().Where(d => !d.IsDeleted).OrderByDescending(d => d.CreationDate).AsNoTracking().AsQueryable();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().Where(d => !d.IsDeleted).OrderByDescending(d => d.CreationDate).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(d => d.Id == id&&!d.IsDeleted);
            //await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {


            await _context.Set<T>().AddAsync(entity);
            //  added = await Save();

            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            //  added = await Save();
            return entities;
        }




        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }
        public async Task<List<T>> DeleteRangeAsync(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return entities;
        }

        public async Task<bool> SoftDeleteRangeAsync(List<Guid> entities)
        {
            foreach (var Id in entities)
            {
                var row = await GetByIdAsync(Id);
                var IsDeleted = row.GetType().GetProperty("IsDeleted");
                IsDeleted.SetValue(row, true);
                await UpdateAsync(row);
            }

            return true;
        }

        public async Task<bool> SoftDelete(Guid Id)
        {
            var row = await GetByIdAsync(Id);
            var IsDeleted = row.GetType().GetProperty("IsDeleted");
            IsDeleted.SetValue(row, true);

            await UpdateAsync(row);
            return true;
        }

    }
}
