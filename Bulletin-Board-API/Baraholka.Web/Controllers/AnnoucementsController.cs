using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Services.Models;
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
            PageDataContainer<AnnoucementModel> pagedObject = await _annoucementService.GetAnnoucements(filterArgs, pageArgs, sortingArgs);

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
            AnnoucementModel annoucementDto = await _annoucementService.GetAnnoucement(id);

            if (annoucementDto == null)
            {
                return NotFound($"No annoucement with id: {id}");
            }

            return Ok(annoucementDto);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> Add([FromForm] AnnoucementCreateModel annoucementDto)
        {
            bool exists = await _annoucementService.BrandCategoryExists(annoucementDto.BrandCategoryId);

            if (exists)
            {
                int userId = User.GetUserID();
                AnnoucementModel annoucement = await _annoucementService.CreateAnnoucement(annoucementDto, userId);
                return CreatedAtAction(nameof(GetById), new { id = annoucement.Id }, annoucement);
            }

            return BadRequest("BrandCategory Id does not exist");
        }

        [Authorize(Roles = "Member")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            AnnoucementModel annoucement = await _annoucementService.GetAnnoucement(id);

            if (annoucement == null)
            {
                return BadRequest($"No annoucement with id: {id}");
            }

            int currentUser = User.GetUserID();

            if (currentUser != annoucement.UserId)
            {
                return StatusCode(403, "You are not allowed to delete other user's annoucement!");
            }

            await _annoucementService.DeleteAnnoucement(id);

            return Ok();
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] AnnoucementUpdateModel annoucementDto)
        {
            AnnoucementModel annoucement = await _annoucementService.GetAnnoucement(annoucementDto.AnnoucementId);

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
                return BadRequest("BrandCategory does not exist");
            }

            AnnoucementModel updatedAnnoucement = await _annoucementService.UpdateAnnoucement(annoucementDto, currentUser);

            return CreatedAtAction(nameof(GetById), new { id = updatedAnnoucement.Id }, updatedAnnoucement);
        }
    }
}