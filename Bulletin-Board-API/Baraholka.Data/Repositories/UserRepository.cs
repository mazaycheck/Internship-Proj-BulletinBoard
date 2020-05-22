using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var includes = new string[] { $"{nameof(Town)}" };
            var conditions = new List<Expression<Func<User, bool>>>
            {
                u => u.Id == id
            };
            var user = await GetSingle(includes, conditions);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<PageDataContainer<UserDto>> GetPagedUsers(string filter, PageArguments pageArguments)
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

            var pagedUsers = await GetPagedData(includes, filters, orderParameters, pageArguments);
            return _mapper.Map<PageDataContainer<UserDto>>(pagedUsers);
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await GetFirst(x => x.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UserExists(string email)
        {
            return await Exists(x => x.Email == email);
        }

        public async Task DeactivateUser(int userId)
        {
            var includes = new string[]
            {
                $"{nameof(User.UserRoles)}",
            };
            var user = await FindById(userId, includes);
            user.UserRoles = new List<UserRole>();
            user.LockoutEnabled = true;
            user.LockoutEnd = new DateTime(2100, 12, 31);
            await Save();
        }

        public async Task ActivateUser(int userId)
        {
            var includes = new string[]
            {
                $"{nameof(User.UserRoles)}",
            };
            var user = await FindById(userId, includes);
            user.UserRoles = new List<UserRole>() { new UserRole() { RoleId = 3, UserId = userId } };
            user.LockoutEnabled = true;
            user.LockoutEnd = null;
            await Save();
        }
    }
}