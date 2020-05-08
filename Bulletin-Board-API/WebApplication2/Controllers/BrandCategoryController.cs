using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data.Dtos;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandCategoryController : ControllerBase
    {
        private readonly IBrandCategoryService _brandCategoryService;

        public BrandCategoryController(IBrandCategoryService brandCategoryService)
        {
            _brandCategoryService = brandCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string category, string brand)
        {
            var data = await _brandCategoryService.GetAll(category, brand);
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _brandCategoryService.GetById(id));
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandCategoryForCreateDto brandCategoryForCreate)
        {
            try
            {
                BrandCategoryForViewDto newBrandCategory = await _brandCategoryService.CreateRelation(brandCategoryForCreate);
                return StatusCode(201, newBrandCategory);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
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
                await _brandCategoryService.DeleteRelation(id);
                return Ok($"Removed successfully relation with id: {id}");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}
