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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string filter)
        {
            List<CategoryModel> allCategories = await _categoryService.GetAllCategories(filter);
            if (allCategories == null)
            {
                return NoContent();
            }

            return Ok(allCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            CategoryModel category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound($"No such category with id: {id}");
            }
            return Ok(category);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateModel categoryDto)
        {
            if (await _categoryService.Exists(categoryDto.Title))
            {
                return Conflict("Such category already exists");
            }
            CategoryModel category = await _categoryService.CreateCategory(categoryDto);
            return StatusCode(201, category);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryUpdateModel categoryForUpdate)
        {
            CategoryModel categoryFromDb = await _categoryService.GetCategory(categoryForUpdate.CategoryId);
            if (categoryFromDb.Title != categoryForUpdate.Title)
            {
                if (await _categoryService.Exists(categoryForUpdate.Title))
                {
                    return Conflict("This category already exists");
                }
            }

            CategoryModel category = await _categoryService.UpdateCategory(categoryForUpdate);
            return StatusCode(201, category);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound($"No such category with id: {id}");
            }

            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}