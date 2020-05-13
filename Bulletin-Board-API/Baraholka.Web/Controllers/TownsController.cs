using Baraholka.Data.Dtos;
using Baraholka.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TownsController : ControllerBase
    {
        private readonly ITownService _townService;

        public TownsController(ITownService townService)
        {
            _townService = townService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("public")]
        public async Task<IActionResult> Get()
        {
            List<TownForPublicViewDto> towns = await _townService.GetTownsForPublic();
            if (towns == null)
            {
                return NoContent();
            }
            return Ok(towns);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            TownForAdminViewDto town = await _townService.GetTownForAdmin(id);
            if (town == null)
            {
                return NotFound($"No town with id: {id}");
            }
            return Ok(town);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TownForCreateDto town)
        {
            try
            {
                TownForAdminViewDto newTown = await _townService.CreateTown(town);
                return StatusCode(201, newTown);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TownForUpdateDto town)
        {
            try
            {
                TownForAdminViewDto updatedTown = await _townService.UpdateTown(town);
                return StatusCode(201, updatedTown);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _townService.DeleteTown(id);
                return Ok();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}