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

        Task<List<T>> GetAll(string[] references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams);

        Task<List<T>> GetAll(List<Expression<Func<T, object>>> references, List<Expression<Func<T, bool>>> filters, List<OrderParams<T>> orderParams);

        Task<T> GetSingle(params Expression<Func<T, bool>>[] expressions);

        Task<T> GetSingle(Expression<Func<T, bool>> condition, string[] includes);

        Task<T> GetById(int id);

        Task<T> GetById(int id, List<Expression<Func<T, object>>> references, List<Expression<Func<T, IEnumerable<object>>>> collections);

        Task<bool> Exists(int id);

        Task<bool> Exists(params Expression<Func<T, bool>>[] expressions);

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(T entity);

        Task<int> Save();
    }
}