using Baraholka.Data.Dtos;
using System.Threading.Tasks;
using Baraholka.Services.Models;

namespace Baraholka.Services
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);

        Task<UserPublicModel> Register(UserRegisterDto userRegisterDto);

        Task<bool> UserExists(string email);
    }
}