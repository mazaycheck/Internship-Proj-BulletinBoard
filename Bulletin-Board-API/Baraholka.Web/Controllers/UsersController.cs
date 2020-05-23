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
        public async Task<IActionResult> GetUsers([FromQuery] PageArguments pageArguments, [FromQuery] string filter)
        {
            PageDataContainer<UserDto> pagedUserDtos = await _userService.GetUsers(pageArguments, filter);

            if (pagedUserDtos == null)
            {
                return NoContent();
            }
            PageDataContainer<UserAuditWebModel> pagedUserAuditModels = _mapper.Map<PageDataContainer<UserAuditWebModel>>(pagedUserDtos);

            return Ok(pagedUserAuditModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            UserDto userDto = await _userService.GetUser(id);
            if (userDto == null)
            {
                return NotFound();
            }
            UserPublicWebModel userPublicModel = _mapper.Map<UserPublicWebModel>(userDto);

            return Ok(userPublicModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("deactivate/{id}")]
        public async Task<ActionResult> DeactivateUser([FromRoute] int id)
        {
            UserDto userDto = await _userService.GetUser(id);
            if (userDto == null)
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
            UserDto userDto = await _userService.GetUser(id);
            if (userDto == null)
            {
                return NotFound("No such user");
            }

            await _userService.ActivateUser(id);
            return Ok();
        }
    }
}