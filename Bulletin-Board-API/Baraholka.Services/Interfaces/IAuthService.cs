using Baraholka.Data.Dtos;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);

        Task<UserDto> Register(UserDto userRegisterDto, string password);

        Task<bool> UserExists(string email);
    }
}