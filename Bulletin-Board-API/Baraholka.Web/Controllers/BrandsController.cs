using Baraholka.Data.Dtos;
using Baraholka.Services;
using Baraholka.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baraholka.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]BrandFilterArguments filterArgs,
            [FromQuery]PageArguments pageArgs, [FromQuery]SortingArguments sortingArgs)
        {
            PageDataContainer<BrandModel> brands = await _brandService.GetAllBrands(filterArgs, pageArgs, sortingArgs);
            if (brands != null)
            {
                return Ok(brands);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            BrandModel brandDto = await _brandService.GetBrand(id);
            if (brandDto == null)
            {
                return NotFound($"No such brand with id: {id}");
            }

            return Ok(brandDto);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandCreateModel brand)
        {
            if (await _brandService.BrandExist(brand.Title))
            {
                return Conflict("Such brand already exists");
            }
            BrandModel newBrand = await _brandService.CreateBrand(brand);
            return StatusCode(201, newBrand);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] BrandUpdateModel brandForUpdate)
        {
            var brandFromDb = await _brandService.GetBrand(brandForUpdate.BrandId);

            if (brandFromDb == null)
            {
                return NotFound("No such brand");
            }

            if (await _brandService.UpdatedBrandExists(brandForUpdate))
            {
                return Conflict("This brand already exists");
            }

            BrandModel updatedBrand = await _brandService.UpdateBrand(brandForUpdate);
            return Ok(updatedBrand);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brandFromDb = await _brandService.GetBrand(id);
            if (brandFromDb == null)
            {
                return NotFound("No such brand");
            }
            await _brandService.DeleteBrand(id);

            return Ok();
        }
    }
}