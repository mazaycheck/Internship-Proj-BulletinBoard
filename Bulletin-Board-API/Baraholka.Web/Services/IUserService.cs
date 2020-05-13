using Baraholka.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Web.Services
{
    public interface IUserService
    {
        Task DeleteUser(int id);

        Task<UserForPublicDetail> GetUser(int id);

        Task<PageDataContainer<UserForModeratorView>> GetUsers([FromQuery] PageArguments pageArguments, [FromQuery] string query);
    }
}