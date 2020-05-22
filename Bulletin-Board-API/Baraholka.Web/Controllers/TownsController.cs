using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Web.Models;
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
        private readonly IMapper _mapper;

        public TownsController(ITownService townService, IMapper mapper)
        {
            _townService = townService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("public")]
        public async Task<IActionResult> Get()
        {
            List<TownDto> towns = await _townService.GetAllTowns();
            if (towns == null)
            {
                return NoContent();
            }
            List<TownWebModel> townsModels = _mapper.Map<List<TownDto>, List<TownWebModel>>(towns);
            return Ok(townsModels);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            TownDto townDto = await _townService.GetTown(id);
            if (townDto == null)
            {
                return NotFound($"No town with id: {id}");
            }

            return Ok(_mapper.Map<TownWebModel>(townDto));
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TownCreateModel townCreateModel)
        {
            if (await _townService.Exists(townCreateModel.Title))
            {
                return Conflict($"Such town already exists");
            }
            TownDto townCreateDto = _mapper.Map<TownDto>(townCreateModel);
            TownDto newTownDto = await _townService.CreateTown(townCreateDto);
            TownWebModel newTownModel = _mapper.Map<TownWebModel>(newTownDto);
            return CreatedAtAction(nameof(Get), new { id = newTownModel.TownId }, newTownModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TownUpdateModel townUpdateModel)
        {
            TownDto townFromDb = await _townService.GetTown(townUpdateModel.TownId);
            if (townFromDb == null)
            {
                return NotFound($"No such town with id: {townUpdateModel.TownId}");
            }
            if (townFromDb.Title != townUpdateModel.Title && await _townService.Exists(townUpdateModel.Title))
            {
                return Conflict($"Such town already exists with title {townUpdateModel.Title} ");
            }
            TownDto townCreateDto = _mapper.Map<TownDto>(townUpdateModel);
            TownDto updatedTown = await _townService.UpdateTown(townCreateDto);
            TownWebModel updatedTownModel = _mapper.Map<TownWebModel>(updatedTown);

            return CreatedAtAction(nameof(Get), new { id = updatedTownModel.TownId }, updatedTownModel);
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