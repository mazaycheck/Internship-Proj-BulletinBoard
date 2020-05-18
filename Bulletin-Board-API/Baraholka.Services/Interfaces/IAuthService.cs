using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);

        Task<UserForPublicDetail> Register(UserRegisterDto userRegisterDto);
        
        Task<bool> UserExists(string email);
    }
}