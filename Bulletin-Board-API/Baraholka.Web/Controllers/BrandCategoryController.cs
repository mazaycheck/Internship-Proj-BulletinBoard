using Baraholka.Data.Dtos;
using Baraholka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandCategoryController : ControllerBase
    {
        private readonly IBrandCategoryService _brandCategoryService;

        public BrandCategoryController(IBrandCategoryService brandCategoryService)
        {
            _brandCategoryService = brandCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string category, string brand)
        {
            var brandCategoryRelations = await _brandCategoryService.GetAllRelations(category, brand);

            if (brandCategoryRelations.Count == 0)
            {
                return NoContent();
            }

            return Ok(brandCategoryRelations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var brandCategoryRelation = await _brandCategoryService.GetRelationById(id);

            if (brandCategoryRelation == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandCategoryForCreateDto brandCategoryForCreate)
        {
            try
            {
                BrandCategoryForViewDto newBrandCategory = await _brandCategoryService.CreateRelation(brandCategoryForCreate);

                return StatusCode(201, newBrandCategory);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
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
                await _brandCategoryService.DeleteRelation(id);
                return Ok($"Removed relation with id: {id}");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}