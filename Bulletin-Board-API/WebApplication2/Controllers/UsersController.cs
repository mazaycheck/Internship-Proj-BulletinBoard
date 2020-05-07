using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Helpers;
using WebApplication2.Models;


namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository context, IMapper mapper)
        {
            _repo = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PaginateParams paginateParams, [FromQuery] string query)
        {
            var users =  _repo.GetAll()
                .ProjectTo<UserForModeratorView>(_mapper.ConfigurationProvider);
            var filtered = users.Where(x => x.Email.Contains(query ?? "") || x.UserName.Contains(query ?? ""));
            var pageData = await Paged<UserForModeratorView>.Paginate(filtered, paginateParams);

            return Ok(pageData);
        }



        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromDb = await _repo.GetById(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            var usereDto = _mapper.Map<UserForPublicDetail>(userFromDb);
  
            return Ok(usereDto);
        }

        [Authorize(Policy = ("ReqModOrAdmin"))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            await _repo.Update(user);
            await _repo.Save();

            return Ok();
        }

        [Authorize(Policy = ("ReqModOrAdmin"))]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _repo.GetById(id);
            if(user == null)
            {
                return NotFound(id);
            }
            await _repo.Delete(user);
            await _repo.Save();
            return Ok(user);
        }

    }
}
