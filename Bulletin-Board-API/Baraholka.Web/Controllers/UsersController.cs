using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Services;
using Baraholka.Web.Models;
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
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PageArguments paginateParams, [FromQuery] string query)
        {
            PageDataContainer<UserDto> usersDto = await _userService.GetUsers(paginateParams, query);

            if (usersDto == null)
            {
                return NoContent();
            }
            PageDataContainer<UserAdminModel> pagedUsersAdminModel = _mapper.Map<PageDataContainer<UserAdminModel>>(usersDto);
            return Ok(pagedUsersAdminModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            UserDto userFromDb = await _userService.GetUser(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            UserPublicWebModel userWebModel = _mapper.Map<UserPublicWebModel>(userFromDb);
            return Ok(userWebModel);
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