using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication2.Helpers;

namespace WebApplication2.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly IPageService<T> _pageService;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async virtual Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return;
        }

        public virtual async Task Delete(T entity)
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
            query = references.Aggregate(query, (current, navigationProp) => query.Include(navigationProp));
            query = expressions.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.AnyAsync();
        }

        public async Task<T> FindFirst(params Expression<Func<T, bool>>[] expressions)
        {
            var query = _context.Set<T>().AsQueryable();
            query = expressions.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.FirstOrDefaultAsync();
        }

        public virtual IQueryable<T> GetQueryableSet()
        {
            return _context.Set<T>();
        }

        public virtual async Task<T> GetById(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<T> GetById(int id, List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (references != null)
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

        public async Task<T> GetSingle(Expression<Func<T, bool>> condition, string[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {                     
                query = includes.Aggregate(query, (current, include) => query.Include(include));            
            }
            var entity = await query.SingleAsync(condition);
            return entity;
        }

        public async Task<List<T>> GetAll(List<Expression<Func<T, object>>> includes)
        {
            var query = _context.Set<T>().AsQueryable();

            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAll(List<Expression<Func<T, object>>> includes, List<Expression<Func<T, bool>>> filters)
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

        public async virtual Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<List<T>> GetAll(string[] references)
        {
            var query = _context.Set<T>().AsQueryable();

            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters)
        {
            var query = _context.Set<T>().AsQueryable();

            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters, List<Expression<Func<T, object>>> orderParams, bool descending = false)
        {
            var query = _context.Set<T>().AsQueryable();
            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            IOrderedQueryable<T> orderedQuery = descending ? query.OrderByDescending(orderParams[0]) : query.OrderBy(orderParams[0]);

            if (orderParams.Count > 1)
            {
                if (descending)
                    orderedQuery = orderParams.Aggregate(orderedQuery, (current, orderParam) => current.ThenByDescending(orderParam));
                else
                    orderedQuery = orderParams.Aggregate(orderedQuery, (current, orderParam) => current.ThenBy(orderParam));
            }

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAll(List<Expression<Func<T, object>>> references, List<Expression<Func<T, bool>>> filters, List<Expression<Func<T, object>>> orderParams, bool descending = false)
        {
            var query = _context.Set<T>().AsQueryable();
            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            IOrderedQueryable<T> orderedQuery = descending ? query.OrderByDescending(orderParams[0]) : query.OrderBy(orderParams[0]);

            if (orderParams.Count > 1)
            {
                if (descending)
                    orderedQuery = orderParams.Aggregate(orderedQuery, (current, orderParam) => current.ThenByDescending(orderParam));
                else
                    orderedQuery = orderParams.Aggregate(orderedQuery, (current, orderParam) => current.ThenBy(orderParam));
            }

            return await query.ToListAsync();
        }
    }
}