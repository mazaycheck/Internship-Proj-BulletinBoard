using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
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
       
        public BrandsController(IBrandService brandService, IPageService<Brand> pageService)
        {
            _brandService = brandService;       
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]BrandFilterArguments filterArgs,
            [FromQuery]PageArguments pageArgs, [FromQuery]SortingArguments sortingArgs)
        {
            PageDataContainer<BrandForViewDto> brands = await _brandService.GetAllBrands(filterArgs, pageArgs, sortingArgs);
            if (brands != null)
            {
                return Ok(brands);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            BrandForViewDto brandDto =  await _brandService.GetBrand(id);
            if(brandDto == null)
            {
                return NotFound(id);
            }
                                            
            return Ok(brandDto);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandForCreateDto brand)
        {
            try
            {
                BrandForViewDto newBrand = await _brandService.CreateBrand(brand);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);                 
            }            
           
            return StatusCode(201, brand);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] BrandForUpdateDto brandForUpdate)
        {
            try
            {
                await _brandService.UpdateBrand(brandForUpdate);
                return Ok();
            }
            catch (NullReferenceException ex)
            {

                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _brandService.DeleteBrand(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }         
            
            return Ok();
        }
    }
}