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
        public async Task<IActionResult> Post([FromBody] TownCreateModel townCreateModel)
        {
            if (await _townService.Exists(townCreateModel.Title))
            {
                return Conflict($"Such town already exists");
            }

            TownModel newTown = await _townService.CreateTown(townCreateModel);
            return StatusCode(201, newTown);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TownUpdateModel townUpdateModel)
        {
            TownModel townFromDb = await _townService.GetTown(townUpdateModel.TownId);
            if (townFromDb == null)
            {
                return NotFound($"No such town with id: {townUpdateModel.TownId}");
            }
            if (townFromDb.Title != townUpdateModel.Title && await _townService.Exists(townUpdateModel.Title))
            {
                return Conflict($"Such town already exists with title {townUpdateModel.Title} ");
            }

            TownModel updatedTown = await _townService.UpdateTown(townUpdateModel);

            return CreatedAtAction(nameof(Get), new { id = updatedTown.TownId }, updatedTown);
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