using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, 
            UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    return user;
                }
                
            }
            throw new UnauthorizedAccessException("Cannot login");
            

        }

        public async Task<UserForPublicDetail> Register(UserRegisterDto userRegisterDto)
        {

            var userEmail = userRegisterDto.Email;
            if(await _userManager.FindByEmailAsync(userEmail) != null)
            {
                throw  new ArgumentException("User already exists");                
            }

            var newUser = _mapper.Map<User>(userRegisterDto);

            var result = await _userManager.CreateAsync(newUser, userRegisterDto.Password);

            if (result.Succeeded) 
            {
                return _mapper.Map<UserForPublicDetail>(newUser);
            }

            else
            {
                return null;
            }
            

        }


        public async Task<string> CreateToken(User user)
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
    }
}
