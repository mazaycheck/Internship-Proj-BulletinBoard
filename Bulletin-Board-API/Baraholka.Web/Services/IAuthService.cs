using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Threading.Tasks;

namespace Baraholka.Web.Services
{
    public interface IAuthService
    {
        Task<User> Login(string email, string password);

        Task<UserForPublicDetail> Register(UserRegisterDto userRegisterDto);

        Task<string> CreateToken(User user);
    }
}