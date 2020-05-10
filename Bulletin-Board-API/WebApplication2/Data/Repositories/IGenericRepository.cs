using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication2.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetQueryableSet();
        Task<List<T>> GetAll(string[] references);
        Task<List<T>> GetAll(List<Expression<Func<T, object>>> references);
        //Task<List<T>> GetAllInclude(List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections);
        Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters);
        Task<List<T>> GetAll(List<Expression<Func<T, object>>> references, List<Expression<Func<T, bool>>> filters);
        //Task<List<T>> GetAllIncludeFilter(List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections, List<Expression<Func<T, bool>>> filters);
        Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters, List<Expression<Func<T, object>>> orderParams, bool descending = false);
        Task<List<T>> GetAll(List<Expression<Func<T, object>>> references, List<Expression<Func<T, bool>>> filters, List<Expression<Func<T, object>>> orderParams, bool descending = false);    
        //Task<List<T>> GetAllIncludeFilterOrderBy(List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections, List<Expression<Func<T, bool>>> filters, Expression<Func<T, object>> orderBy, bool descending);
        Task<T> GetSingle(Expression<Func<T, bool>> condition, string[] includes);
        Task<T> GetById(int id);
        Task<T> GetById(int id, List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections);        
        Task<bool> Exists(int id);
        Task<bool> Exists(params Expression<Func<T, bool>>[] expressions);        
        Task<bool> Exists(List<Expression<Func<T, bool>>> expressions, List<Expression<Func<T, object>>> references);
        //Task<bool> ExistsWhereInclude(List<Expression<Func<T, bool>>> expressions, List<Expression<Func<T, object>>> references, List<Expression<Func<T, object>>> collections);
        Task<T> FindFirst(params Expression<Func<T, bool>>[] expressions);
        Task Create(T entity);        
        Task Update(T entity);
        Task Delete(T entity);
        Task<int> Save();
    }
}
