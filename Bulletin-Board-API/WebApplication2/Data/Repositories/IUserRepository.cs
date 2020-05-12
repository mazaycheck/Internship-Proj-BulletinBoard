using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IOrderedQueryable<User> PrepareUsersForPaging(string filter);

        Task<User> GetByEmail(string email);

        Task<bool> UserExists(string email);
    }
}