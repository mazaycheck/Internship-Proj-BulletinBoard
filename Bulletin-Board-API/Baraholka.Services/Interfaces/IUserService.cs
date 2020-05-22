using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IUserService
    {
        Task ActivateUser(int id);

        Task DeactivateUser(int id);

        Task<UserDto> GetUser(int id);

        Task<PageDataContainer<UserDto>> GetUsers([FromQuery] PageArguments pageArguments, [FromQuery] string query);
    }
}