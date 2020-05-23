using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Services;
using Baraholka.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnoucementsController : ControllerBase
    {
        private readonly IAnnoucementService _annoucementService;
        private readonly IMapper _mapper;

        public AnnoucementsController(IAnnoucementService annoucementService, IMapper mapper)
        {
            _annoucementService = annoucementService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll([FromQuery]AnnoucementFilterArguments filterArgs,
            [FromQuery]PageArguments pageArgs, [FromQuery]SortingArguments sortingArgs)
        {
            PageDataContainer<AnnoucementDto> pagedAnnoucementDtos = await _annoucementService.GetAnnoucements(filterArgs, pageArgs, sortingArgs);

            if (pagedAnnoucementDtos == null)
            {
                return NoContent();
            }
            PageDataContainer<AnnoucementWebModel> pagedWebAnnoucements = _mapper.Map<PageDataContainer<AnnoucementWebModel>>(pagedAnnoucementDtos);

            return Ok(pagedWebAnnoucements);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            AnnoucementDto annoucementDto = await _annoucementService.GetAnnoucement(id);

            if (annoucementDto == null)
            {
                return NotFound($"No annoucement with id: {id}");
            }
            AnnoucementWebModel annoucementWebModel = _mapper.Map<AnnoucementWebModel>(annoucementDto);

            return Ok(annoucementWebModel);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> Add([FromForm] AnnoucementCreateModel annoucementCreateModel)
        {
            AnnoucementDto annoucementDto = _mapper.Map<AnnoucementDto>(annoucementCreateModel);

            annoucementDto.UserId = User.GetUserID();

            AnnoucementDto createdAnnoucementDto = await _annoucementService.CreateAnnoucement(annoucementDto, annoucementCreateModel.Photo);

            AnnoucementWebModel createdAnnoucementModel = _mapper.Map<AnnoucementWebModel>(createdAnnoucementDto);

            return CreatedAtAction(nameof(GetById), new { id = createdAnnoucementDto.AnnoucementId }, createdAnnoucementDto);
        }

        [Authorize(Roles = "Member")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            AnnoucementDto annoucement = await _annoucementService.GetAnnoucement(id);

            if (annoucement == null)
            {
                return BadRequest($"No annoucement with id: {id}");
            }

            int currentUser = User.GetUserID();

            if (currentUser != annoucement.UserId)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, "You are not allowed to delete other user's annoucement!");
            }

            await _annoucementService.DeleteAnnoucement(id);

            return Ok();
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] AnnoucementUpdateModel annoucementUpdateModel)
        {
            AnnoucementDto annoucementDto = await _annoucementService.GetAnnoucement(annoucementUpdateModel.AnnoucementId);

            if (annoucementDto == null)
            {
                return BadRequest($"No annoucement with id: {annoucementUpdateModel.AnnoucementId}");
            }

            int currentUser = User.GetUserID();

            if (currentUser != annoucementDto.UserId)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, "You are not allowed to update other user's annoucement!");
            }

            AnnoucementDto annoucementUpdateDto = _mapper.Map<AnnoucementDto>(annoucementUpdateModel);
            annoucementUpdateDto.UserId = currentUser;

            AnnoucementDto updatedAnnoucement = await _annoucementService.UpdateAnnoucement(annoucementUpdateDto, annoucementUpdateModel.Photo);

            return CreatedAtAction(nameof(GetById), new { id = updatedAnnoucement.AnnoucementId }, updatedAnnoucement);
        }
    }
}