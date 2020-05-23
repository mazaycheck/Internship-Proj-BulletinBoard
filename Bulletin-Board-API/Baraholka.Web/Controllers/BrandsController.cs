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
            PageDataContainer<BrandDto> pagedBrandDtos = await _brandService.GetAllBrands(filterArgs, pageArgs, sortingArgs);

            if (pagedBrandDtos == null)
            {
                return NoContent();
            }

            PageDataContainer<BrandWebModel> pagedBrandModels = _mapper.Map<PageDataContainer<BrandWebModel>>(pagedBrandDtos);

            return Ok(pagedBrandModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            BrandDto brandDto = await _brandService.GetBrand(id);
            if (brandDto == null)
            {
                return NotFound($"No such brand with id: {id}");
            }
            BrandWebModel brandModel = _mapper.Map<BrandWebModel>(brandDto);

            return Ok(brandModel);
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
            BrandWebModel brandModel = _mapper.Map<BrandWebModel>(createdBrandDto);

            return CreatedAtAction(nameof(Get), new { id = brandModel.BrandId }, brandModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] BrandUpdateModel brandUpdateModel)
        {
            BrandDto brandFromDb = await _brandService.GetBrand(brandUpdateModel.BrandId);

            if (brandFromDb == null)
            {
                return NotFound("No such brand");
            }

            BrandDto brandUpdateDto = _mapper.Map<BrandDto>(brandUpdateModel);

            if (await _brandService.UpdatedBrandExists(brandUpdateDto))
            {
                return Conflict("This brand already exists");
            }

            BrandDto updatedBrandDto = await _brandService.UpdateBrand(brandUpdateDto, brandUpdateModel.Categories);
            BrandWebModel updatedBrandModel = _mapper.Map<BrandWebModel>(updatedBrandDto);

            return Ok(updatedBrandModel);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            BrandDto brandDto = await _brandService.GetBrand(id);
            if (brandDto == null)
            {
                return NotFound("No such brand");
            }
            await _brandService.DeleteBrand(id);

            return Ok();
        }
    }
}