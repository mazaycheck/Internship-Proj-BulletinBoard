using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Services.Models;
using System;
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

        public async Task<PageDataContainer<UserAdminModel>> GetUsers(PageArguments pageArguments, string query)
        {
            PageDataContainer<UserDto> pagedUsers = await _userRepository.GetPagedUsers(query, pageArguments);

            if (pagedUsers.PageData.Count > 0)
            {
                PageDataContainer<UserAdminModel> pagedUserDtos = _mapper.Map<PageDataContainer<UserAdminModel>>(pagedUsers);
                return pagedUserDtos;
            }

            return null;
        }

        public async Task<UserPublicModel> GetUser(int id)
        {
            UserDto user = await _userRepository.GetUser(id);
            return _mapper.Map<UserPublicModel>(user);
        }

        public async Task DeactivateUser(int id)
        {
            await _userRepository.DeactivateUser(id);
        }

        public async Task ActivateUser(int id)
        {
            await _userRepository.ActivateUser(id);
        }
    }
}