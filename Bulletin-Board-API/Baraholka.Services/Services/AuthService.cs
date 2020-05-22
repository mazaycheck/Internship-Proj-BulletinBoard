using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthService(IConfiguration configuration,
            UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    return await CreateToken(user);
                }
            }
            return null;
        }

        public async Task<UserDto> Register(UserDto userRegisterDto, string password)
        {
            var userEmail = userRegisterDto.Email;
            if (await _userManager.FindByEmailAsync(userEmail) != null)
            {
                throw new ArgumentException("User already exists");
            }

            var newUser = _mapper.Map<User>(userRegisterDto);

            var result = await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                return _mapper.Map<UserDto>(newUser);
            }
            else
            {
                return null;
            }
        }

        private async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescritor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UserExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}