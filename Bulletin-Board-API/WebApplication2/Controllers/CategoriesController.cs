using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category> _repo;

        public CategoriesController(IGenericRepository<Category> repo)
        {
            _repo = repo;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories =  await _repo.GetAll()
                .Include(x=>x.BrandCategories)
                    .ThenInclude(p => p.Brand)
                .Select(x => new CategoryForViewDto
                { 
                    CategoryId = x.CategoryId, 
                    Title = x.Title, 
                    Brands = x.BrandCategories.Select(x => x.Brand.Title)
                })
                .ToListAsync();
            return Ok(categories);
        }



        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _repo.GetAll().Where(x => x.CategoryId == id)
             .Include(x => x.BrandCategories)
                 .ThenInclude(p => p.Brand)
             .Select(x => new CategoryForViewDto
             {
                 CategoryId = x.CategoryId,
                 Title = x.Title,
                 Brands = x.BrandCategories.Select(x => x.Brand.Title)
             })
             .SingleOrDefaultAsync();

            if (category == null)
            {
                return NotFound(id);
            }
            return Ok(category);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repo.Create(cat);
            await _repo.Save();
            return StatusCode(201, cat);            
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Category cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

         
            await _repo.Update(cat);
            await _repo.Save();
            return StatusCode(201, cat);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _repo.GetById(id);
            if (category == null)
            {
                return NotFound(id);
            }
            await _repo.Delete(category);
            await _repo.Save();
            return Ok(category);
        }
    }
}
