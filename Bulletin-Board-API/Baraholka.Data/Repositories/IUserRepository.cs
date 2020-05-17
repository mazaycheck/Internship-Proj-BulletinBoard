using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmail(string email);

        Task<bool> UserExists(string email);

        Task<PageDataContainer<User>> GetPagedUsers(string filter, PageArguments pageArguments);
    }
}