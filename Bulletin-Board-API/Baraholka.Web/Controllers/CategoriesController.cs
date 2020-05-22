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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string filter)
        {
            List<CategoryDto> allCategories = await _categoryService.GetAllCategories(filter);
            if (allCategories == null)
            {
                return NoContent();
            }
            List<CategoryModel> categoriesWebModel = _mapper.Map<List<CategoryDto>, List<CategoryModel>>(allCategories);
            return Ok(categoriesWebModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            CategoryDto category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound($"No such category with id: {id}");
            }
            CategoryModel categoryWebModel = _mapper.Map<CategoryModel>(category);
            return Ok(categoryWebModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateModel categoryCreateModel)
        {
            if (await _categoryService.Exists(categoryCreateModel.Title))
            {
                return Conflict("Such category already exists");
            }
            CategoryDto categoryDto = _mapper.Map<CategoryDto>(categoryCreateModel);
            CategoryDto createdCategory = await _categoryService.CreateCategory(categoryDto);

            return StatusCode(201, _mapper.Map<CategoryModel>(createdCategory));
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryUpdateModel categoryUpdateModel)
        {
            CategoryDto categoryFromDb = await _categoryService.GetCategory(categoryUpdateModel.CategoryId);
            if (categoryFromDb.Title != categoryUpdateModel.Title)
            {
                if (await _categoryService.Exists(categoryUpdateModel.Title))
                {
                    return Conflict("This category already exists");
                }
            }

            CategoryDto categoryUpdateDto = _mapper.Map<CategoryDto>(categoryUpdateModel);
            CategoryDto updatedCategory = await _categoryService.UpdateCategory(categoryUpdateDto);
            return StatusCode(201, _mapper.Map<CategoryModel>(updatedCategory));
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