using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Services.Models;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        public async Task<PageDataContainer<BrandModel>> GetAllBrands(BrandFilterArguments filterArguments,
            PageArguments pageArguments, SortingArguments sortingArguments)
        {
            PageDataContainer<BrandDto> pagedBrands = await _brandRepo.GetPagedBrands(filterArguments, sortingArguments, pageArguments);

            if (pagedBrands.PageData.Count > 0)
            {
                PageDataContainer<BrandModel> pagedBrandDtos = _mapper.Map<PageDataContainer<BrandModel>>(pagedBrands);
                return pagedBrandDtos;
            }

            return null;
        }

        public async Task<BrandModel> GetBrand(int id)
        {
            BrandDto brand = await _brandRepo.GetBrand(id);
            return _mapper.Map<BrandModel>(brand);
        }

        public async Task<BrandModel> CreateBrand(BrandCreateModel brandForCreate)
        {
            BrandDto brand = _mapper.Map<BrandDto>(brandForCreate);
            BrandDto newBrand = await _brandRepo.CreateBrand(brand);
            return _mapper.Map<BrandModel>(newBrand);
        }

        public async Task DeleteBrand(int brandId)
        {
            await _brandRepo.DeleteBrand(brandId);
        }

        public async Task<BrandModel> UpdateBrand(BrandUpdateModel brandUpdateModel)
        {
            BrandDto brandDto = _mapper.Map<BrandDto>(brandUpdateModel);

            BrandDto updatedBrandFromDb = await _brandRepo.UpdateBrand(brandDto, brandUpdateModel.Categories);

            return _mapper.Map<BrandModel>(updatedBrandFromDb);
        }

        public async Task<bool> UpdatedBrandExists(BrandUpdateModel brandForUpdate)
        {
            BrandDto brandFromDb = await _brandRepo.GetBrand(brandForUpdate.BrandId);

            if (brandFromDb.Title != brandForUpdate.Title && await BrandExist(brandForUpdate.Title))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> BrandExist(string title)
        {
            return await _brandRepo.Exists(brand => brand.Title.ToLower().Equals(title.ToLower()));
        }
    }
}