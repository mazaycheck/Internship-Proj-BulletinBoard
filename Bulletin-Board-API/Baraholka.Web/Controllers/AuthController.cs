using Baraholka.Data.Dtos;
using Baraholka.Services;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
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
        public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegisterDto)
        {
            if (await _authService.UserExists(userRegisterDto.Email))
            {
                return Conflict("Such user already exists");
            }
            var createdUser = await _authService.Register(userRegisterDto);
            return Ok(createdUser);
        }
    }
}