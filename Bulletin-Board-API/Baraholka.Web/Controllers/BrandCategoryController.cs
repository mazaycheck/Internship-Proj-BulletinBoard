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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandCategoryController : ControllerBase
    {
        private readonly IBrandCategoryService _brandCategoryService;
        private readonly IMapper _mapper;

        public BrandCategoryController(IBrandCategoryService brandCategoryService, IMapper mapper)
        {
            _brandCategoryService = brandCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string category, string brand)
        {
            List<BrandCategoryDto> brandCategoryDtos = await _brandCategoryService.GetAllRelations(category, brand);

            if (brandCategoryDtos.Count == 0)
            {
                return NoContent();
            }

            List<BrandCategoryWebModel> brandCategoryModels = _mapper
                .Map<List<BrandCategoryDto>, List<BrandCategoryWebModel>>(brandCategoryDtos);

            return Ok(brandCategoryModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            BrandCategoryDto brandCategoryDto = await _brandCategoryService.GetRelationById(id);

            if (brandCategoryDto == null)
            {
                return NotFound();
            }
            BrandCategoryWebModel brandCategoryModel = _mapper.Map<BrandCategoryWebModel>(brandCategoryDto);

            return Ok(brandCategoryModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandCategoryCreateModel brandCategoryCreateModel)
        {
            CategoryDto categoryDto = await _brandCategoryService.GetCategory(brandCategoryCreateModel.Category);
            if (categoryDto == null)
            {
                return NotFound("No such category");
            }

            BrandDto brandDto = await _brandCategoryService.GetBrand(brandCategoryCreateModel.Brand);
            if (brandDto == null)
            {
                return NotFound("No such brand");
            }

            if (await _brandCategoryService.BrandCategoryExists(brandDto.BrandId, categoryDto.CategoryId))
            {
                return Conflict($"This relation already exists");
            }

            BrandCategoryDto newBrandCategoryDto = await _brandCategoryService.CreateRelation(brandDto.BrandId, categoryDto.CategoryId);
            BrandCategoryWebModel newBrandCategoryModel = _mapper.Map<BrandCategoryWebModel>(newBrandCategoryDto);
            return CreatedAtAction(nameof(Get), new { id = newBrandCategoryModel.BrandCategoryId }, newBrandCategoryModel);
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