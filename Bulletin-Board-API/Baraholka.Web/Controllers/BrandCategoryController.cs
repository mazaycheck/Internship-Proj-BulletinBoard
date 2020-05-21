using Baraholka.Data.Dtos;
using Baraholka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            return Ok(brandCategoryRelation);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandCategoryCreateModel brandCategoryForCreate)
        {
            var category = await _brandCategoryService.GetCategory(brandCategoryForCreate.Category);
            if (category == null)
            {
                return NotFound("No such category");
            }

            var brand = await _brandCategoryService.GetBrand(brandCategoryForCreate.Brand);
            if (brand == null)
            {
                return NotFound("No such brand");
            }

            if (await _brandCategoryService.BrandCategoryExists(brand.BrandId, category.CategoryId))
            {
                return Conflict($"This relation already exists");
            }

            BrandCategoryModel newBrandCategory = await _brandCategoryService.CreateRelation(brand.BrandId, category.CategoryId);
            return CreatedAtAction(nameof(Get), new { id = newBrandCategory.BrandCategoryId }, newBrandCategory);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _brandCategoryService.BrandCategoryExists(id))
            {
                return BadRequest($"Brand-Category relation with id: {id} does not exist");
            }

            await _brandCategoryService.DeleteRelation(id);
            return Ok($"Removed relation with id: {id}");
        }
    }
}