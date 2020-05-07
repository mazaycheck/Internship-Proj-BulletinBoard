using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();                
        Task<T> GetById(int id);
        Task Create(T entity);        
        Task Update(T entity);
        Task Delete(T entity);
        Task<int> Save();        
        Task<bool> Exists(int id);
    }
}
