using Baraholka.Data.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
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

        public async virtual Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<int> Save()
        {
            var count = await _context.SaveChangesAsync();
            return count;
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
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            query = expressions.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.AnyAsync();
        }

        public virtual async Task<T> FindById(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<T> FindById(int id, List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections)
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

        public async Task<T> FindById(int id, string[] includes)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (includes != null)
            {
                foreach (var reference in includes)
                {
                    await _context.Entry(entity).Navigation(reference).LoadAsync();
                }
            }

            return entity;
        }

        public async Task<T> GetFirst(params Expression<Func<T, bool>>[] filters)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            query = filters.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetFirst(string[] includes, List<Expression<Func<T, bool>>> filters)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => query.Include(include));
            }
            query = filters.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            var entity = await query.FirstOrDefaultAsync();
            return entity;
        }

        public async Task<T> GetSingle(params Expression<Func<T, bool>>[] filters)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            query = filters.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            return await query.SingleOrDefaultAsync();
        }

        public async Task<T> GetSingle(string[] includes, List<Expression<Func<T, bool>>> filters)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            query = filters.Aggregate(query, (current, nextExpression) => current.Where(nextExpression));
            var entity = await query.SingleOrDefaultAsync();
            return entity;
        }

        public IOrderedQueryable<T> GetAllForPaging(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
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

        public async Task<PageDataContainer<T>> GetPagedData(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams, PageArguments pageArguments)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            var firstOrderParam = orderParams[0];
            var orderedQuery = firstOrderParam.Descending ? query.OrderByDescending(firstOrderParam.OrderBy) : query.OrderBy(firstOrderParam.OrderBy);

            if (orderParams.Count > 1)
            {
                orderedQuery = orderParams.Skip(1).Aggregate(orderedQuery, (current, orderParam) => orderParam.Descending ? current.ThenByDescending(orderParam.OrderBy) : current.ThenBy(orderParam.OrderBy));
            }

            return await orderedQuery.GetPageAsync(pageArguments);
        }

        public async Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            query = references.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            query = filters.Aggregate(query, (current, filterProperty) => current.Where(filterProperty));
            var firstOrder = orderParams[0];
            var orderedQuery = firstOrder.Descending ? query.OrderByDescending(firstOrder.OrderBy) : query.OrderBy(firstOrder.OrderBy);

            if (orderParams.Count > 1)
            {
                orderedQuery = orderParams.Skip(1).Aggregate(orderedQuery, (current, orderParam) =>
                    orderParam.Descending ?
                    current.ThenByDescending(orderParam.OrderBy) :
                    current.ThenBy(orderParam.OrderBy));
            }

            return await orderedQuery.ToListAsync();
        }

        public async Task<List<T>> GetAll(List<Expression<Func<T, object>>> references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
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