using Baraholka.Data.Dtos;
using Baraholka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _authService.Login(userLoginDto.Email, userLoginDto.Password);
                var jwtToken = await _authService.CreateToken(user);
                return Ok(new { token = jwtToken });
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegisterDto)
        {
            UserForPublicDetail createdUser;
            try
            {
                createdUser = await _authService.Register(userRegisterDto);
            }
            catch (ArgumentException a)
            {
                return StatusCode(409, a.Message);
            }

            return Ok(createdUser);
        }
    }
}