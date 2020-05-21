using Baraholka.Data.Dtos;
using Baraholka.Services;
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
            UserForPublicDetail userFromDb = await _userService.GetUser(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            return Ok(userFromDb);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userService.FindUserByID(id);
            if (user == null)
            {
                return NotFound("No such user");
            }
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}