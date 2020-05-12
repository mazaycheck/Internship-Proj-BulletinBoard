using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnoucementsController : ControllerBase
    {
        private readonly IAnnoucementService _service;

        public AnnoucementsController(IAnnoucementService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll([FromQuery]AnnoucementFilterArguments filterArgs,
            [FromQuery]PageArguments pageArgs, [FromQuery]SortingArguments sortingArgs)
        {
            PageDataContainer<AnnoucementViewDto> pagedObject = await _service.GetAnnoucements(filterArgs, pageArgs, sortingArgs);
            if (pagedObject != null)
            {
                return Ok(pagedObject);
            }
            else
            {
                return NoContent();
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            AnnoucementViewDto annoucementDto = await _service.GetAnnoucementById(id);
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
            try
            {
                AnnoucementViewDto annoucement = await _service.CreateAnnoucement(annoucementDto);
                return CreatedAtAction("GetById", new { id = annoucement.Id }, annoucement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                await _service.DeleteAnnoucementById(id);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] AnnoucementUpdateDto annoucementDto)
        {
            try
            {
                AnnoucementViewDto annoucement = await _service.UpdateAnnoucement(annoucementDto);
                return CreatedAtAction("GetById", new { id = annoucement.Id }, annoucement);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}