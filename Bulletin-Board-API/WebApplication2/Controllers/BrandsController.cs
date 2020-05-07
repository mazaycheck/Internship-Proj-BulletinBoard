using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IGenericRepository<Brand> _repo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public BrandsController(IGenericRepository<Brand> repo, IMapper mapperConfig, AppDbContext context)
        {
            
            _repo = repo;
            _mapper = mapperConfig;
            _context = context;
        }

        
        [HttpGet]        
        public async Task<IActionResult> Get([FromQuery]AnnoucementFilter filterOptions,
            [FromQuery]PaginateParams paginateParams, [FromQuery]OrderParams orderParams)
        {            
            var brands = _repo.GetAll().OrderBy(x => x.Title).Include(x => x.BrandCategories).ThenInclude(x => x.Category)
                .Select(x => new BrandForViewDto
                {
                    BrandId = x.BrandId,
                    Title = x.Title,
                    Categories = x.BrandCategories.Select(x => x.Category.Title)
                });
            var filtered = brands.Where(x => x.Title.Contains(filterOptions.Query ?? ""));
            var paginatedData = await Paged<BrandForViewDto>.Paginate(filtered, paginateParams);
            return Ok(paginatedData);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var brand = await _repo.GetById(id);
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
            
            await _repo.Create(brand);
            await _repo.Save();       
            return StatusCode(201, brand);
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [Route("updateCategories")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandForUpdateDto brandForUpdate )
        {
            var brand = await _repo.GetAll()
                .Include(x => x.BrandCategories)
                    .ThenInclude(x => x.Category)
                .Where(x => x.BrandId == brandForUpdate.BrandId).SingleOrDefaultAsync();
            if(brand == null)
            {
                return NotFound("Not found such brand");
            }

            if(brand.Title != brandForUpdate.Title)
            {
                brand.Title = brandForUpdate.Title;
                await _repo.Save();
            }

            var categoriesOfBrandFromDb = brand.BrandCategories.Select(x => x.Category.Title).ToArray();

            if (!categoriesOfBrandFromDb.SequenceEqual(brandForUpdate.Categories)) { 
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

            if(brandCategoriesToAdd.Count() > 0)
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

            await _repo.Update(brand);
            await _repo.Save();
            return StatusCode(201, brand);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _repo.GetById(id);
            if (brand == null)
            {
                return NotFound(id);
            }
            await _repo.Delete(brand);
            await _repo.Save();
            return Ok(brand);
        }
    }
}
