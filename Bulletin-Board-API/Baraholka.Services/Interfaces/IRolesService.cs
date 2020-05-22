using Baraholka.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IRolesService
    {
        Task<List<string>> GetRoles();

        Task<UserAdminModel> UpdateUserRoles(UserRolesUpdateModel userRolesForModifyDto);

        Task<bool> UserExists(string email);
    }
}