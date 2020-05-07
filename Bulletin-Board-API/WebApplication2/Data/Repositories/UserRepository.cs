using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;

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

        public IQueryable<User> GetAll()
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

  


    }
}
