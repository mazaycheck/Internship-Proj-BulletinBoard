using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;

namespace Baraholka.Web.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

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

        public async Task<T> GetSingle(params Expression<Func<T, bool>>[] expressions)
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

        public IOrderedQueryable<T> GetAllForPaging(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams)
        {
            var query = _context.Set<T>().AsQueryable();
            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            var firstOrderParam = orderParams[0];
            var orderedQuery = firstOrderParam.Descending ? query.OrderByDescending(firstOrderParam.OrderBy) : query.OrderBy(firstOrderParam.OrderBy);

            if (orderParams.Count > 1)
            {
                orderedQuery = orderParams.Skip(1).Aggregate(orderedQuery, (current, orderParam) => orderParam.Descending ? current.ThenByDescending(orderParam.OrderBy) : current.ThenBy(orderParam.OrderBy));
            }

            return orderedQuery;
        }

        public async Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams)
        {
            var query = _context.Set<T>().AsQueryable();
            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            var firstOrder = orderParams[0];
            var orderedQuery = firstOrder.Descending ? query.OrderByDescending(firstOrder.OrderBy) : query.OrderBy(firstOrder.OrderBy);

            if (orderParams.Count > 1)
            {
                orderedQuery = orderParams.Skip(1).Aggregate(orderedQuery, (current, orderParam) => orderParam.Descending ? current.ThenByDescending(orderParam.OrderBy) : current.ThenBy(orderParam.OrderBy));
            }

            return await orderedQuery.ToListAsync();
        }

        public async Task<List<T>> GetAll(List<Expression<Func<T, object>>> references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams)
        {
            var query = _context.Set<T>().AsQueryable();
            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            var firstOrder = orderParams[0];
            var orderedQuery = firstOrder.Descending ? query.OrderByDescending(firstOrder.OrderBy) : query.OrderBy(firstOrder.OrderBy);

            if (orderParams.Count > 1)
            {
                orderedQuery = orderParams.Skip(1).Aggregate(orderedQuery, (current, orderParam) => orderParam.Descending ? current.ThenByDescending(orderParam.OrderBy) : current.ThenBy(orderParam.OrderBy));
            }

            return await orderedQuery.ToListAsync();
        }
    }
}