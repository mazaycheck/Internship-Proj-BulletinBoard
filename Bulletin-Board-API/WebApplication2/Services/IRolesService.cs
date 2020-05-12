using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;

namespace WebApplication2.Services
{
    public interface IRolesService
    {
        Task<List<string>> GetRoles();

        Task<UserForModeratorView> UpdateUserRoles(UserRolesForModifyDto userRolesForModifyDto);
    }
}