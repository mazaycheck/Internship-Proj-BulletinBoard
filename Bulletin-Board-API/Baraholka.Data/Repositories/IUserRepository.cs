using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserDto> GetByEmail(string email);

        Task<bool> UserExists(string email);

        Task<PageDataContainer<UserDto>> GetPagedUsers(string filter, PageArguments pageArguments);

        Task<UserDto> GetUser(int id);

        Task DeactivateUser(int userId);

        Task ActivateUser(int userId);
    }
}