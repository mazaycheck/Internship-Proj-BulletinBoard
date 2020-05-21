using Baraholka.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IUserService
    {
        Task DeleteUser(int id);

        Task<UserServiceDto> FindUserByID(int id);

        Task<UserForPublicDetail> GetUser(int id);

        Task<PageDataContainer<UserAdminModel>> GetUsers([FromQuery] PageArguments pageArguments, [FromQuery] string query);
    }
}