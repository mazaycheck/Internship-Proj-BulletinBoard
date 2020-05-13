using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;
using Baraholka.Domain.Models;

namespace Baraholka.Web.Services
{
    public interface IAuthService
    {
        Task<User> Login(string email, string password);

        Task<UserForPublicDetail> Register(UserRegisterDto userRegisterDto);

        Task<string> CreateToken(User user);
    }
}