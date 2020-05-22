using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginModel userLoginDto)
        {
            var jwtToken = await _authService.Login(userLoginDto.Email, userLoginDto.Password);
            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                return Ok(new { token = jwtToken });
            }

            return Unauthorized("Could not login");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterModel userRegisterModel)
        {
            if (await _authService.UserExists(userRegisterModel.Email))
            {
                return Conflict("Such user already exists");
            }
            UserDto userRegisterDto = _mapper.Map<UserDto>(userRegisterModel);
            UserDto createdUser = await _authService.Register(userRegisterDto, userRegisterModel.Password);
            UserPublicWebModel createdUserModel = _mapper.Map<UserPublicWebModel>(createdUser);

            return Ok(createdUserModel);
        }
    }
}