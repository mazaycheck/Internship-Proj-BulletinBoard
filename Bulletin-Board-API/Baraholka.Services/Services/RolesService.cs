using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class RolesService : IRolesService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RolesService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<List<string>> GetRoles()
        {
            return await _roleManager.Roles.Select(x => x.Name).ToListAsync();
        }

        public async Task<UserDto> UpdateUserRoles(string email, string[] roles)
        {
            var newRoles = roles;
            var user = await _userManager.FindByEmailAsync(email);

            foreach (var role in newRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new ArgumentException($"Role: {role} does not exist!");
                }
            }

            var userCurrentRoles = (await _userManager.GetRolesAsync(user)).ToArray();
            await AddOrDeleteRoles(user, newRoles, userCurrentRoles);

            IList<string> updatedRoles = await _userManager.GetRolesAsync(user);

            UserDto userDto = _mapper.Map<UserDto>(user);

            userDto.Roles = updatedRoles;

            return userDto;
        }

        private async Task AddOrDeleteRoles(User user, string[] newRoles, string[] userCurrentRoles)
        {
            var result = await _userManager.AddToRolesAsync(user, newRoles.Except(userCurrentRoles));

            if (!result.Succeeded)
            {
                throw new Exception("Could not add new roles");
            }
            result = await _userManager.RemoveFromRolesAsync(user, userCurrentRoles.Except(newRoles));

            if (!result.Succeeded)
            {
                throw new Exception("Could not remove roles");
            }
        }

        public async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == email);
        }
    }
}