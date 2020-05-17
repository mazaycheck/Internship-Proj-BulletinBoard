using Baraholka.Data.Dtos;
using Baraholka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            TownServiceDto town = await _townService.FindTown(id);
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
            if (await _townService.Exists(town.Title))
            {
                return Conflict($"Such town already exists");
            }

            TownServiceDto newTown = await _townService.CreateTown(town);
            return StatusCode(201, newTown);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TownForUpdateDto town)
        {
            TownServiceDto updatedTown = await _townService.UpdateTown(town);
            return StatusCode(201, updatedTown);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var town = await _townService.FindTown(id);
            if (town == null)
            {
                return NotFound("No such town");
            }
            await _townService.DeleteTown(town);
            return Ok();
        }
    }
}