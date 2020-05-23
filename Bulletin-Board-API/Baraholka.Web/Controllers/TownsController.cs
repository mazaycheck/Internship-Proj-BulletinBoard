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
            List<TownDto> townDtos = await _townService.GetAllTowns();
            if (townDtos == null)
            {
                return NoContent();
            }
            List<TownWebModel> townModels = _mapper.Map<List<TownDto>, List<TownWebModel>>(townDtos);

            return Ok(townModels);
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
            TownDto createdTownDto = await _townService.CreateTown(townCreateDto);
            TownWebModel createdTownModel = _mapper.Map<TownWebModel>(createdTownDto);

            return CreatedAtAction(nameof(Get), new { id = createdTownModel.TownId }, createdTownModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TownUpdateModel townUpdateModel)
        {
            TownDto townDto = await _townService.GetTown(townUpdateModel.TownId);
            if (townDto == null)
            {
                return NotFound($"No such town with id: {townUpdateModel.TownId}");
            }
            if (townDto.Title != townUpdateModel.Title && await _townService.Exists(townUpdateModel.Title))
            {
                return Conflict($"Such town already exists with title {townUpdateModel.Title} ");
            }
            TownDto townCreateDto = _mapper.Map<TownDto>(townUpdateModel);
            TownDto updatedTownDto = await _townService.UpdateTown(townCreateDto);
            TownWebModel updatedTownModel = _mapper.Map<TownWebModel>(updatedTownDto);

            return CreatedAtAction(nameof(Get), new { id = updatedTownModel.TownId }, updatedTownModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            TownDto townDto = await _townService.GetTown(id);

            if (townDto == null)
            {
                return NotFound("No such town");
            }

            await _townService.DeleteTown(id);

            return Ok();
        }
    }
}