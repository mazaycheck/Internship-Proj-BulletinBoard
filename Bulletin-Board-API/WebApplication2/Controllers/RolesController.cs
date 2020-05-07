using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("roleslist")]
        public async Task<IActionResult> Get()
        {
            var listOfRoles  = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            return Ok(listOfRoles);
        }


        [HttpPost]
        [Route("editroles")]
        public async Task<IActionResult> EditRoles([FromBody] UserRolesForModifyDto userRolesForModifyDto)
        {
            var email = userRolesForModifyDto.Email;
            var newroles = userRolesForModifyDto.NewRoles;
            var user = await _userManager.FindByEmailAsync(email);
            

            foreach (var role in newroles)
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"Role: {role} does not exist!");
                }
            }

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.AddToRolesAsync(user, newroles.Except(userRoles));
                
                if (!result.Succeeded)
                {
                    return BadRequest("Could not update roles");
                }
                result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(newroles));

                if (!result.Succeeded)
                {
                    return BadRequest("Could not update roles");
                }
            }
            var updatedRoles =  await _userManager.GetRolesAsync(user);
            var userToReturn = _mapper.Map<UserForModeratorView>(user);
            userToReturn.Roles = updatedRoles.ToArray<string>();
            return Ok(userToReturn);
        }
    }
}