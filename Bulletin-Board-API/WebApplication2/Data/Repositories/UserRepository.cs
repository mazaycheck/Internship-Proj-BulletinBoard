using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication2.Helpers;

namespace WebApplication2.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(User entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("User null");
            }            
            await _context.Users.AddAsync(entity);            
            //await _context.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {   
            _context.Users.Remove(user);
            return;
        }

        public async Task<bool> Exists(int entityId)
        {
            return await _context.Users.AnyAsync(x => x.Id == entityId);
        }

        public Task<bool> Exists(params Expression<Func<User, bool>>[] expressions)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(List<Expression<Func<User, bool>>> expressions, List<Expression<Func<User, object>>> references)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindFirst(params Expression<Func<User, bool>>[] expressions)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllInclude(params Expression<Func<User, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll(List<Expression<Func<User, object>>> includes, List<Expression<Func<User, bool>>> filters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetQueryableSet()
        {
            return _context.Users.AsNoTracking().Include(x => x.Town).Include(x => x.UserRoles);            
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetById(int id)
        {            
            return await _context.Users.Include(x => x.Town).Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public Task<User> GetByIdInclude(int id, string[] references, string[] collections = null)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(int id, List<Expression<Func<User, object>>> references, List<Expression<Func<User, IEnumerable<object>>>> collections = null)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Save()
        {
            var rowncount = await _context.SaveChangesAsync();
            return rowncount;
        }

        public async Task Update(User entity)
        {
            var userToUpdate = _context.Users.Find(entity.Id);
            _context.Entry(userToUpdate).CurrentValues.SetValues(entity);
           
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public Task<List<User>> GetAll(string[] references)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll(List<Expression<Func<User, object>>> references)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll(string[] references, List<Expression<Func<User, bool>>> filters)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll(string[] references, List<Expression<Func<User, bool>>> filters, List<Expression<Func<User, object>>> orderParams, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll(List<Expression<Func<User, object>>> references, List<Expression<Func<User, bool>>> filters, List<Expression<Func<User, object>>> orderParams, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<PageDataContainer<User>> GetPageData(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PageDataContainer<User>> GetPageData(string[] references, List<Expression<Func<User, bool>>> filters, List<Expression<Func<User, object>>> orderParams, bool descending, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PageDataContainer<User>> GetPageData(List<Expression<Func<User, object>>> references, List<Expression<Func<User, bool>>> filters, List<Expression<Func<User, object>>> orderParams, bool descending, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(int id, string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetSingle(Expression<Func<User, bool>> condition, string[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
