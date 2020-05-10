using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Helpers;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IGenericRepository<Brand> _brandRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public BrandsController(IBrandService brandService, IGenericRepository<Brand> brandRepo, IGenericRepository<Category> categoryRepo, IGenericRepository<BrandCategory> brandCategoryRepo, IMapper mapperConfig, AppDbContext context)
        {
            _brandService = brandService;
            _brandRepo = brandRepo;
            _mapper = mapperConfig;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]AnnoucementFilter filterOptions,
            [FromQuery]PaginateParams paginateParams, [FromQuery]OrderParams orderParams)
        {
            PageService<BrandForViewDto> brands = await _brandService.GetAllBrands(filterOptions, paginateParams, orderParams);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var brand = await _brandRepo.GetById(id);
            if (brand == null)
            {
                return NotFound(id);
            }
            return Ok(brand);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _brandRepo.Create(brand);
            await _brandRepo.Save();
            return StatusCode(201, brand);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [Route("updateCategories")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandForUpdateDto brandForUpdate)
        {
            var brand = await _brandRepo.GetQueryableSet()
                .Include(x => x.BrandCategories)
                    .ThenInclude(x => x.Category)
                .Where(x => x.BrandId == brandForUpdate.BrandId).SingleOrDefaultAsync();
            if (brand == null)
            {
                return NotFound("Not found such brand");
            }

            if (brand.Title != brandForUpdate.Title)
            {
                brand.Title = brandForUpdate.Title;
                await _brandRepo.Save();
            }

            var categoriesOfBrandFromDb = brand.BrandCategories.Select(x => x.Category.Title).ToArray();

            if (!categoriesOfBrandFromDb.SequenceEqual(brandForUpdate.Categories))
            {
                await AddOrRemoveCategoriesOfBrand(brand, brandForUpdate.Categories, categoriesOfBrandFromDb);
            }

            return Ok();
        }

        private async Task AddOrRemoveCategoriesOfBrand(Brand brand, string[] newCategories, string[] oldCategories)
        {
            var brandId = brand.BrandId;

            var categoriesToAdd = newCategories.Except(oldCategories).Select(x => _context.Categories.Where(p => p.Title == x).SingleOrDefault());
            var brandCategoriesToAdd = categoriesToAdd.Select(x => new BrandCategory() { BrandId = brandId, CategoryId = x.CategoryId });

            var categoriesToRemove = oldCategories.Except(newCategories).Select(x => _context.Categories.Where(p => p.Title == x).SingleOrDefault());
            var brandCategoriesToRemove = categoriesToRemove.Select(x => _context.BrandCategories.Where(p => p.CategoryId == x.CategoryId && p.BrandId == brandId).SingleOrDefault());

            if (brandCategoriesToAdd.Count() > 0)
                _context.BrandCategories.AddRange(brandCategoriesToAdd);
            if (brandCategoriesToRemove.Count() > 0)
                _context.BrandCategories.RemoveRange(brandCategoriesToRemove);

            await _context.SaveChangesAsync();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _brandRepo.Update(brand);
            await _brandRepo.Save();
            return StatusCode(201, brand);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepo.GetById(id);
            if (brand == null)
            {
                return NotFound(id);
            }
            await _brandRepo.Delete(brand);
            await _brandRepo.Save();
            return Ok(brand);
        }
    }
}