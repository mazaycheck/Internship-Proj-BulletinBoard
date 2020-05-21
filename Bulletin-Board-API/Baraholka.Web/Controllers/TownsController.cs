using Baraholka.Services;
using Baraholka.Services.Models;
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
            List<TownModel> towns = await _townService.GetAllTowns();
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
            TownModel town = await _townService.GetTown(id);
            if (town == null)
            {
                return NotFound($"No town with id: {id}");
            }
            return Ok(town);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TownCreateModel town)
        {
            if (await _townService.Exists(town.Title))
            {
                return Conflict($"Such town already exists");
            }

            TownModel newTown = await _townService.CreateTown(town);
            return StatusCode(201, newTown);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TownUpdateModel newTown)
        {
            TownModel townFromDb = await _townService.GetTown(newTown.TownId);
            if (townFromDb == null)
            {
                return NotFound($"No such town with id: {newTown.TownId}");
            }
            if (townFromDb.Title != newTown.Title && await _townService.Exists(newTown.Title))
            {
                return Conflict($"Such town already exists with title {newTown.Title} ");
            }

            TownModel updatedTown = await _townService.UpdateTown(newTown);
            return StatusCode(201, updatedTown);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var town = await _townService.GetTown(id);
            if (town == null)
            {
                return NotFound("No such town");
            }
            await _townService.DeleteTown(id);

            return Ok();
        }
    }
}