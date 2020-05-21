using Baraholka.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IOrderedQueryable<T> GetAllForPaging(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams);

        Task<PageDataContainer<T>> GetPagedData(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams, PageArguments pageArguments);

        Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams);

        Task<List<T>> GetAll(List<Expression<Func<T, object>>> references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams);

        Task<T> GetFirst(params Expression<Func<T, bool>>[] expressions);

        Task<T> GetFirst(string[] includes, List<Expression<Func<T, bool>>> filters);

        Task<T> GetSingle(params Expression<Func<T, bool>>[] filters);

        Task<T> GetSingle(string[] includes, List<Expression<Func<T, bool>>> filters);

        Task<T> FindById(int id);

        Task<T> FindById(int id, string[] includes);

        Task<T> FindById(int id, List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections);

        Task<bool> Exists(params Expression<Func<T, bool>>[] expressions);

        Task Create(T entity);

        Task Update(T entity);

        Task<T> CreateAndReturn(T entity);

        Task<T> UpdateAndReturn(T entity);

        Task Delete(T entity);

        Task<int> Save();
        Task UpdateRange(IEnumerable<T> entities);
    }
}