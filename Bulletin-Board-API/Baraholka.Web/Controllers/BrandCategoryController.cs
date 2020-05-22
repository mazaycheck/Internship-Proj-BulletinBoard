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
            var brandCategoryRelations = await _brandCategoryService.GetAllRelations(category, brand);

            if (brandCategoryRelations.Count == 0)
            {
                return NoContent();
            }

            List<BrandCategoryWebModel> brandCategoryWebModels = _mapper
                .Map<List<BrandCategoryDto>, List<BrandCategoryWebModel>>(brandCategoryRelations);

            return Ok(brandCategoryWebModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            BrandCategoryDto brandCategoryRelation = await _brandCategoryService.GetRelationById(id);

            if (brandCategoryRelation == null)
            {
                return NotFound();
            }
            BrandCategoryWebModel brandCategoryWebModel = _mapper.Map<BrandCategoryWebModel>(brandCategoryRelation);
            return Ok(brandCategoryWebModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandCategoryCreateModel brandCategoryForCreate)
        {
            CategoryDto category = await _brandCategoryService.GetCategory(brandCategoryForCreate.Category);
            if (category == null)
            {
                return NotFound("No such category");
            }

            BrandDto brand = await _brandCategoryService.GetBrand(brandCategoryForCreate.Brand);
            if (brand == null)
            {
                return NotFound("No such brand");
            }

            if (await _brandCategoryService.BrandCategoryExists(brand.BrandId, category.CategoryId))
            {
                return Conflict($"This relation already exists");
            }

            BrandCategoryDto newBrandCategory = await _brandCategoryService.CreateRelation(brand.BrandId, category.CategoryId);
            BrandCategoryWebModel newBrandCategoryModel = _mapper.Map<BrandCategoryWebModel>(newBrandCategory);
            return CreatedAtAction(nameof(Get), new { id = newBrandCategory.BrandCategoryId }, newBrandCategoryModel);
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