using System.Linq;
using System.Threading.Tasks;
using Baraholka.Domain.Models;

namespace Baraholka.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IOrderedQueryable<User> PrepareUsersForPaging(string filter);

        Task<User> GetByEmail(string email);

        Task<bool> UserExists(string email);
    }
}