using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IRolesService
    {
        Task<List<string>> GetRoles();

        Task<UserForModeratorView> UpdateUserRoles(UserRolesForModifyDto userRolesForModifyDto);

        Task<bool> UserExists(string email);
    }
}