using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication2.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return;
        }

        public async Task Delete(T entity)
        {
            
            _context.Set<T>().Remove(entity);
            return;
        }

        public async Task<bool> Exists(int entityId)
        {
            var entity = await _context.Set<T>().FindAsync(entityId);
            if (entity != null)
            {
                return true;
            }
            return false;
        }

 

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

  

        public async Task<int> Save()
        {
            var count = await _context.SaveChangesAsync();
            return count;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }



    }
}
