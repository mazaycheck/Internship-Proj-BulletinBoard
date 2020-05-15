using Baraholka.Data.Dtos;
using Baraholka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnoucementsController : ControllerBase
    {
        private readonly IAnnoucementService _annoucementService;

        public AnnoucementsController(IAnnoucementService annoucementService)
        {
            _annoucementService = annoucementService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll([FromQuery]AnnoucementFilterArguments filterArgs,
            [FromQuery]PageArguments pageArgs, [FromQuery]SortingArguments sortingArgs)
        {
            PageDataContainer<AnnoucementViewDto> pagedObject = await _annoucementService.GetAnnoucements(filterArgs, pageArgs, sortingArgs);
            if (pagedObject == null)
            {
                return NoContent();
            }

            return Ok(pagedObject);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            AnnoucementViewDto annoucementDto = await _annoucementService.GetAnnoucementForViewById(id);
            if (annoucementDto == null)
            {
                return NotFound($"No annoucement with id: {id}");
            }

            return Ok(annoucementDto);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> Add([FromForm] AnnoucementCreateDto annoucementDto)
        {
            bool result = await _annoucementService.BrandCategoryExists(annoucementDto.BrandCategoryId);
            if (result)
            {
                int userId = User.GetUserID();
                AnnoucementViewDto annoucement = await _annoucementService.CreateAnnoucement(annoucementDto, userId);
                return CreatedAtAction(nameof(GetById), new { id = annoucement.Id }, annoucement);
            }

            return BadRequest("BrandCategory Id does not exist");
        }

        [Authorize(Roles = "Member")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            AnnoucementCheckDto annoucement = await _annoucementService.GetAnnoucementForValidateById(id);

            if (annoucement == null)
            {
                return BadRequest($"No annoucement with id: {id}");
            }

            int currentUser = User.GetUserID();

            if (currentUser != annoucement.UserId)
            {
                return StatusCode(403, "You are not allowed to delete other user's annoucement!");
            }

            await _annoucementService.DeleteAnnoucementById(id);

            return Ok();
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] AnnoucementUpdateDto annoucementDto)
        {
            AnnoucementCheckDto annoucement = await _annoucementService.GetAnnoucementForValidateById(annoucementDto.AnnoucementId);

            if (annoucement == null)
            {
                return BadRequest($"No annoucement with id: {annoucementDto.AnnoucementId}");
            }

            int currentUser = User.GetUserID();

            if (currentUser != annoucement.UserId)
            {
                return StatusCode(403, "You are not allowed to update other user's annoucement!");
            }

            bool result = await _annoucementService.BrandCategoryExists(annoucementDto.BrandCategoryId);

            if (!result)
            {
                return BadRequest("BrandCategory Id does not exist");
            }

            AnnoucementViewDto updatedAnnoucement = await _annoucementService.UpdateAnnoucement(annoucementDto);

            return CreatedAtAction(nameof(GetById), new { id = updatedAnnoucement.Id }, updatedAnnoucement);
        }
    }
}