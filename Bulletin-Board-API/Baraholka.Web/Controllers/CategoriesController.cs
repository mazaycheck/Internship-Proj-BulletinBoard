using Baraholka.Data.Dtos;
using Baraholka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            List<CategoryForViewDto> allCategories = await _categoryService.GetAllCategories(filter);
            if (allCategories == null)
            {
                return NoContent();
            }

            return Ok(allCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            CategoryForViewDto category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound($"No such category with id: {id}");
            }
            return Ok(category);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryForCreateDto cat)
        {
            try
            {
                CategoryForViewDto category = await _categoryService.CreateCategory(cat);
                return StatusCode(201, category);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryForCreateDto categoryForUpdate)
        {
            try
            {
                CategoryForViewDto category = await _categoryService.CreateCategory(categoryForUpdate);
                return StatusCode(201, category);
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
                await _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound($"No such category with id: {id}");
            }
        }
    }
}