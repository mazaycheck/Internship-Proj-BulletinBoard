using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Web.Services
{
    public interface IRolesService
    {
        Task<List<string>> GetRoles();

        Task<UserForModeratorView> UpdateUserRoles(UserRolesForModifyDto userRolesForModifyDto);
    }
}