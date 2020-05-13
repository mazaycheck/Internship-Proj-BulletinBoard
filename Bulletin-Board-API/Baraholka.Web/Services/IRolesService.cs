using System.Collections.Generic;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;

namespace Baraholka.Web.Services
{
    public interface IRolesService
    {
        Task<List<string>> GetRoles();

        Task<UserForModeratorView> UpdateUserRoles(UserRolesForModifyDto userRolesForModifyDto);
    }
}