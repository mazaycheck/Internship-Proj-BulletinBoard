using Baraholka.Services;
using Baraholka.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        [Route("roleslist")]
        public async Task<IActionResult> Get()
        {
            var listOfRoles = await _rolesService.GetRoles();
            if (listOfRoles == null)
            {
                return NoContent();
            }
            return Ok(listOfRoles);
        }

        [HttpPost]
        [Route("editroles")]
        public async Task<IActionResult> EditRoles([FromBody] UserRolesUpdateModel userRolesForModifyDto)
        {
            var userExists = await _rolesService.UserExists(userRolesForModifyDto.Email);
            if (userExists)
            {
                UserAdminModel updatedUser = await _rolesService.UpdateUserRoles(userRolesForModifyDto);
                return Ok(updatedUser);
            }
            return BadRequest("No such user");
        }
    }
}