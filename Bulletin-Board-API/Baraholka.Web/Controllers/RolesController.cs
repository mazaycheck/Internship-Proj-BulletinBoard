using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;

        public RolesController(IRolesService rolesService, IMapper mapper)
        {
            _rolesService = rolesService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("roleslist")]
        public async Task<IActionResult> Get()
        {
            List<string> listOfRoles = await _rolesService.GetRoles();

            if (listOfRoles == null)
            {
                return NoContent();
            }

            return Ok(listOfRoles);
        }

        [HttpPost]
        [Route("editroles")]
        public async Task<IActionResult> EditRoles([FromBody] UserRolesUpdateModel userRolesUpdateModel)
        {
            bool userExists = await _rolesService.UserExists(userRolesUpdateModel.Email);

            if (!userExists)
            {
                return BadRequest("No such user");
            }

            UserDto updatedUserDto = await _rolesService.UpdateUserRoles(userRolesUpdateModel.Email, userRolesUpdateModel.Roles);
            UserAuditWebModel updatedUserAuditModel = _mapper.Map<UserAuditWebModel>(updatedUserDto);

            return Ok(updatedUserAuditModel);
        }
    }
}