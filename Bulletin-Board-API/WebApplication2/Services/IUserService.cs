using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;

namespace WebApplication2.Services
{
    public interface IUserService
    {
        Task DeleteUser(int id);

        Task<UserForPublicDetail> GetUser(int id);

        Task<PageDataContainer<UserForModeratorView>> GetUsers([FromQuery] PageArguments pageArguments, [FromQuery] string query);
    }
}