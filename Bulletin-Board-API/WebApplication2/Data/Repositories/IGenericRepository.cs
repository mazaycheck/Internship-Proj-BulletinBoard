using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication2.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllQueryable();
        Task<List<T>> GetAllInclude(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllIncludeFilter(List<Expression<Func<T, object>>> includes, List<Expression<Func<T, bool>>> filters);
        Task<T> GetById(int id);
        Task<T> GetByIdInclude(int id, List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections = null);
        Task<bool> Exists(int id);
        Task<bool> Exists(params Expression<Func<T, bool>>[] expressions);
        Task<bool> Exists(List<Expression<Func<T, bool>>> expressions, List<Expression<Func<T, object>>> references);
        Task<T> FindFirst(params Expression<Func<T, bool>>[] expressions);
        Task Create(T entity);        
        Task Update(T entity);
        Task Delete(T entity);
        Task<int> Save();
    }
}
