using Faker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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
            await _context.SaveChangesAsync();
            return;
        }

        public async Task Delete(T entity)
        {
            
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
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

        public async Task<bool> Exists(params Expression<Func<T, bool>>[] expressions)
        {
            var query = _context.Set<T>().AsQueryable();
            query = expressions.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.AnyAsync();
        }

        public async Task<bool> Exists(List<Expression<Func<T, bool>>> expressions, List<Expression<Func<T, object>>> references)
        {
            var query = _context.Set<T>().AsQueryable();
            query = references.Aggregate(query,(current, navigationProp) => query.Include(navigationProp));
            query = expressions.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.AnyAsync();
        }


        public async Task<T> FindFirst(params Expression<Func<T, bool>>[] expressions)
        {
            var query = _context.Set<T>().AsQueryable();
            query = expressions.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.FirstOrDefaultAsync();
        }



        public IQueryable<T> GetAllQueryable()
        {
            return _context.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }



        public async Task<T> GetByIdInclude(int id, List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections = null)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(references != null)
            {
                foreach (var reference in references)
                {
                    await _context.Entry(entity).Reference(reference).LoadAsync();
                }
            }

            if (collections != null)
            {
                foreach (var collection in collections)
                {
                    await _context.Entry(entity).Collection(collection).LoadAsync();
                }                
            }

             return entity;
        }


        public async Task<List<T>> GetAllInclude(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllIncludeFilter(List<Expression<Func<T, object>>> includes, List<Expression<Func<T, bool>>> filters)
        {
            var query = _context.Set<T>().AsQueryable();

            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            
            return await query.ToListAsync();
        }

        public async Task<int> Save()
        {
            var count = await _context.SaveChangesAsync();
            return count;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return;
        }


    }
}
