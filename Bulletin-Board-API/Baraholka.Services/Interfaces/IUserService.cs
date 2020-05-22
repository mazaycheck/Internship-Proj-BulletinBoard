using Baraholka.Data.Dtos;
using Baraholka.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IUserService
    {
        Task ActivateUser(int id);
        Task DeactivateUser(int id);

        Task<UserPublicModel> GetUser(int id);

        Task<PageDataContainer<UserAdminModel>> GetUsers([FromQuery] PageArguments pageArguments, [FromQuery] string query);
    }
}