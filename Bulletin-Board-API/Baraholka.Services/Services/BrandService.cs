using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Data.Repositories;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepo;

        public BrandService(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public async Task<PageDataContainer<BrandDto>> GetAllBrands(BrandFilterArguments filterArguments,
            PageArguments pageArguments, SortingArguments sortingArguments)
        {
            PageDataContainer<BrandDto> pagedBrands = await _brandRepo.GetPagedBrands(filterArguments, sortingArguments, pageArguments);

            if (pagedBrands.PageData.Count > 0)
            {
                return pagedBrands;
            }

            return null;
        }

        public async Task<BrandDto> GetBrand(int id)
        {
            return await _brandRepo.GetBrand(id);
        }

        public async Task<BrandDto> CreateBrand(BrandDto brandCreateDto)
        {
            BrandDto newBrand = await _brandRepo.CreateBrand(brandCreateDto);
            return newBrand;
        }

        public async Task DeleteBrand(int brandId)
        {
            await _brandRepo.DeleteBrand(brandId);
        }

        public async Task<BrandDto> UpdateBrand(BrandDto brandUpdateDto, string[] categories)
        {
            BrandDto updatedBrandFromDb = await _brandRepo.UpdateBrand(brandUpdateDto, categories);

            return updatedBrandFromDb;
        }

        public async Task<bool> UpdatedBrandExists(BrandDto brandForUpdate)
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