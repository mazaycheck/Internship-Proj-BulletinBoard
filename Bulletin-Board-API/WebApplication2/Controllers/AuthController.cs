using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
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
         
            if(!ModelState.IsValid)
            { 
                return BadRequest(ModelState); 
            }
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
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserForPublicDetail createdUser;
            try
            {
                createdUser = await _authService.Register(userRegisterDto);
            }
            catch (ArgumentException a)
            {
                return StatusCode(409,a.Message);                
            }            

            if (createdUser != null)
            {                
                return Ok(createdUser);
            }
            else
            {
                return Problem("Could not register");
            }
        }

        
    }
}
