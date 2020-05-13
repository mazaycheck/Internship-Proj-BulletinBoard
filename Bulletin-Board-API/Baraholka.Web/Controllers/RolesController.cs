using Baraholka.Data.Dtos;
using Baraholka.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> EditRoles([FromBody] UserRolesForModifyDto userRolesForModifyDto)
        {
            try
            {
                UserForModeratorView updatedUser = await _rolesService.UpdateUserRoles(userRolesForModifyDto);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}