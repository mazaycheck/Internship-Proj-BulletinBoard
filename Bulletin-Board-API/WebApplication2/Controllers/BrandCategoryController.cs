using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandCategoryController : ControllerBase
    {
        private readonly BrandCategoryService _brandCategoryService;

        public BrandCategoryController(BrandCategoryService brandCategoryService)
        {
            _brandCategoryService = brandCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string categoryFilter, string brandFilter)
        {
            var data = await _brandCategoryService.GetAll(categoryFilter, brandFilter);
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dataQuery = _repo.GetAll()
                    .Include(x => x.Category).Include(x => x.Brand)
                    .Where(x => x.BrandCategoryId == id);

            var data = await dataQuery
                .Select(x => new BrandCategoryForViewDto()
                {
                    BrandCategoryId = x.BrandCategoryId,
                    CategoryId = x.CategoryId,
                    CategoryTitle = x.Category.Title,
                    BrandId = x.BrandId,
                    BrandTitle = x.Brand.Title
                })
                .SingleOrDefaultAsync();
            return Ok(data);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] BrandCategory entity)
        {
            var exists = await _repo.GetAll()
                .FirstOrDefaultAsync(x => x.CategoryId == entity.CategoryId & x.BrandId == entity.BrandId);
            if (exists != null)
                return BadRequest($"This relation already exists: {entity}");
            await _repo.Create(entity);
            await _repo.Save();
            return StatusCode(201, entity);
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BrandCategory entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var exists = await _repo.GetAll()
                .FirstOrDefaultAsync(x => x.CategoryId == entity.CategoryId & x.BrandId == entity.BrandId);
            if (exists != null)
                return BadRequest($"This relation already exists : {entity}");


            await _repo.Update(entity);
            await _repo.Save();
            return StatusCode(201, entity);
        }







        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brandCategory = await _repo.GetById(id);
            if (brandCategory == null)
            {
                return NotFound(id);
            }
            await _repo.Delete(brandCategory);
            await _repo.Save();
            return Ok(brandCategory);
        }
    }
}
