using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _userRepository = repository;
            _mapper = mapper;
        }

        public async Task<PageDataContainer<UserForModeratorView>> GetUsers(PageArguments pageArguments, string query)
        {
            PageDataContainer<User> pagedUsers = await _userRepository.GetPagedUsers(query, pageArguments);

            if (pagedUsers.PageData.Count > 0)
            {
                PageDataContainer<UserForModeratorView> pagedUserDtos = _mapper.Map<PageDataContainer<UserForModeratorView>>(pagedUsers);
                return pagedUserDtos;
            }

            return null;
        }

        public async Task<UserForPublicDetail> GetUser(int id)
        {
            var includes = new string[] { $"{nameof(Town)}" };
            var conditions = new List<Expression<Func<User, bool>>>
            {
                u => u.Id == id
            };
            var user = await _userRepository.GetSingle(includes, conditions);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserForPublicDetail>(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.FindById(id);
            if (user == null)
            {
                throw new NullReferenceException("No such user");
            }
            await _userRepository.Delete(user);
        }

        public async Task<UserServiceDto> FindUserByID(int id)
        {
            var user = await _userRepository.GetSingle(x => x.Id == id);
            return _mapper.Map<UserServiceDto>(user);
        }
    }
}