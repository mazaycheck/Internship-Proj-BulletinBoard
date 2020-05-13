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

        public async Task<UserForModeratorView> UpdateUserRoles(UserRolesForModifyDto userRolesForModifyDto)
        {
            var email = userRolesForModifyDto.Email;
            var newRoles = userRolesForModifyDto.Roles;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new ArgumentException($"No such user with email: {email}");
            }

            foreach (var role in newRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new ArgumentException($"Role: {role} does not exist!");
                }
            }

            var userCurrentRoles = (await _userManager.GetRolesAsync(user)).ToArray();
            await AddOrDeleteRoles(user, newRoles, userCurrentRoles);

            var updatedRoles = await _userManager.GetRolesAsync(user);
            var updatedUser = _mapper.Map<UserForModeratorView>(user);

            updatedUser.Roles = updatedRoles.ToArray<string>();
            return updatedUser;
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
    }
}