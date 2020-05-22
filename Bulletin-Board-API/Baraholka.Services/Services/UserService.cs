using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public async Task<PageDataContainer<UserDto>> GetUsers(PageArguments pageArguments, string query)
        {
            PageDataContainer<UserDto> pagedUsers = await _userRepository.GetPagedUsers(query, pageArguments);

            if (pagedUsers.PageData.Count > 0)
            {
                return pagedUsers;
            }

            return null;
        }

        public async Task<UserDto> GetUser(int id)
        {
            UserDto user = await _userRepository.GetUser(id);
            return user;
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