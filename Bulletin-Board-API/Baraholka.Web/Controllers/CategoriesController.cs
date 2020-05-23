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
            List<CategoryDto> categoryDtos = await _categoryService.GetAllCategories(filter);
            if (categoryDtos == null)
            {
                return NoContent();
            }
            List<CategoryWebModel> categoryModels = _mapper.Map<List<CategoryDto>, List<CategoryWebModel>>(categoryDtos);

            return Ok(categoryModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            CategoryDto categoryDto = await _categoryService.GetCategory(id);
            if (categoryDto == null)
            {
                return NotFound($"No such category with id: {id}");
            }
            CategoryWebModel categoryModel = _mapper.Map<CategoryWebModel>(categoryDto);

            return Ok(categoryModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateModel categoryCreateModel)
        {
            if (await _categoryService.Exists(categoryCreateModel.Title))
            {
                return Conflict("Such category already exists");
            }
            CategoryDto categoryCreateDto = _mapper.Map<CategoryDto>(categoryCreateModel);
            CategoryDto createdCategoryDto = await _categoryService.CreateCategory(categoryCreateDto);

            return CreatedAtAction(nameof(Get), new { id = createdCategoryDto.CategoryId }, createdCategoryDto);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryUpdateModel categoryUpdateModel)
        {
            CategoryDto categoryDto = await _categoryService.GetCategory(categoryUpdateModel.CategoryId);
            if (categoryDto.Title != categoryUpdateModel.Title)
            {
                if (await _categoryService.Exists(categoryUpdateModel.Title))
                {
                    return Conflict("This category already exists");
                }
            }

            CategoryDto categoryUpdateDto = _mapper.Map<CategoryDto>(categoryUpdateModel);
            CategoryDto updatedCategoryDto = await _categoryService.UpdateCategory(categoryUpdateDto);

            return CreatedAtAction(nameof(Get), new { id = updatedCategoryDto.CategoryId }, updatedCategoryDto);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            CategoryDto categoryDto = await _categoryService.GetCategory(id);
            if (categoryDto == null)
            {
                return NotFound($"No such category with id: {id}");
            }

            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}