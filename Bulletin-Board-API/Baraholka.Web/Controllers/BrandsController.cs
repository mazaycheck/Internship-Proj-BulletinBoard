using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Services;
using Baraholka.Web.Models;
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
        private readonly IMapper _mapper;

        public BrandsController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]BrandFilterArguments filterArgs,
            [FromQuery]PageArguments pageArgs, [FromQuery]SortingArguments sortingArgs)
        {
            PageDataContainer<BrandDto> pagedBrands = await _brandService.GetAllBrands(filterArgs, pageArgs, sortingArgs);

            if (pagedBrands == null)
            {
                return NoContent();
            }

            PageDataContainer<BrandModel> pagedBrandsForWeb = _mapper.Map<PageDataContainer<BrandModel>>(pagedBrands);
            return Ok(pagedBrandsForWeb);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            BrandDto brandDto = await _brandService.GetBrand(id);
            if (brandDto == null)
            {
                return NotFound($"No such brand with id: {id}");
            }
            BrandModel brandWebModel = _mapper.Map<BrandModel>(brandDto);
            return Ok(brandWebModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandCreateModel brandCreateModel)
        {
            if (await _brandService.BrandExist(brandCreateModel.Title))
            {
                return Conflict("Such brand already exists");
            }
            BrandDto brandDto = _mapper.Map<BrandDto>(brandCreateModel);
            BrandDto createdBrandDto = await _brandService.CreateBrand(brandDto);
            BrandModel brandWebModel = _mapper.Map<BrandModel>(createdBrandDto);
            return StatusCode(201, brandWebModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] BrandUpdateModel brandUpdateModel)
        {
            var brandFromDb = await _brandService.GetBrand(brandUpdateModel.BrandId);

            if (brandFromDb == null)
            {
                return NotFound("No such brand");
            }

            BrandDto brandDto = _mapper.Map<BrandDto>(brandUpdateModel);

            if (await _brandService.UpdatedBrandExists(brandDto))
            {
                return Conflict("This brand already exists");
            }

            BrandDto updatedBrand = await _brandService.UpdateBrand(brandDto, brandUpdateModel.Categories);
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