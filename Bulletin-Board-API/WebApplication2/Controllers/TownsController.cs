using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TownsController : ControllerBase
    {
        private readonly IGenericRepository<Town> _repo;

        public TownsController(IGenericRepository<Town> repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var towns = await _repo.GetAll().ToListAsync();
            return Ok(towns);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var town = await _repo.GetById(id);
            if (town == null)
            {
                return NotFound(id);
            }
            return Ok(town);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Town town)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repo.Create(town);
            await _repo.Save();
            return StatusCode(201, town);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Town town)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repo.Update(town);
            await _repo.Save();
            return StatusCode(201, town);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var town = await _repo.GetById(id);
            if (town == null)
            {
                return NotFound(id);
            }
            await _repo.Delete(town);
            await _repo.Save();
            return Ok(town);
        }
    }
}
