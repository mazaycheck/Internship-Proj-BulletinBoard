using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;


namespace Baraholka.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public IOrderedQueryable<User> PrepareUsersForPaging(string filter)
        {
            var filters = new List<Expression<Func<User, bool>>>()
            {
                user => user.UserName.Contains(filter ?? ""),
                user => user.Email.Contains(filter ?? ""),
            };

            var includes = new string[] { $"{nameof(User.UserRoles)}.{nameof(Role)}", $"{nameof(User.Town)}" };

            var orderParameters = new List<OrderParams<User>>()
            {
                new OrderParams<User> { OrderBy = (x) => x.UserName, Descending = false }
            };

            IOrderedQueryable<User> query = GetAllForPaging(includes, filters, orderParameters);

            return query;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await GetSingle(x => x.Email == email);
        }

        public async Task<bool> UserExists(string email)
        {
            return await Exists(x => x.Email == email);
        }
    }
}