using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PageArguments paginateParams, [FromQuery] string query)
        {
            PageDataContainer<UserAdminModel> users = await _userService.GetUsers(paginateParams, query);
            if (users == null)
            {
                return NoContent();
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            UserPublicWebModel userFromDb = await _userService.GetUser(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            return Ok(userFromDb);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("deactivate/{id}")]
        public async Task<ActionResult> DeactivateUser([FromRoute] int id)
        {
            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound("No such user");
            }
            await _userService.DeactivateUser(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("activate/{id}")]
        public async Task<ActionResult> ActivateUser([FromRoute] int id)
        {
            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound("No such user");
            }
            await _userService.ActivateUser(id);
            return Ok();
        }
    }
}