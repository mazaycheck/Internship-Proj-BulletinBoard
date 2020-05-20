using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Baraholka.Services.Models;

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

        public async Task<BrandDto> UpdateBrand(BrandUpdateModel brandForUpdate)
        {
            BrandDto brandForUpdateDto = _mapper.Map<BrandDto>(brandForUpdate);

            BrandDto brandFromDb = await _brandRepo.GetBrand(brandForUpdate.BrandId);

            await UpdateBrandProperties(brandFromDb, brandForUpdateDto);

            await UpdatedAssociatedCategories(brandFromDb, brandForUpdate.Categories);

            brandFromDb = await _brandRepo.GetBrand(brandForUpdate.BrandId);

            return _mapper.Map<BrandDto>(brandFromDb);
        }

        private async Task UpdatedAssociatedCategories(BrandDto brandFromDb, string[] newCategories)
        {
            string[] oldCategories = brandFromDb.BrandCategories.Select(x => x.Category.Title).ToArray();

            if (!oldCategories.SequenceEqual(newCategories))
            {
                await AddOrRemoveCategoriesOfBrand(brandFromDb, newCategories, oldCategories);
            }
        }

        private async Task UpdateBrandProperties(BrandDto brandFromDb, BrandDto brandForUpdateDto)
        {            
            if (brandFromDb.Title != brandForUpdateDto.Title)
            {
                if(!await BrandExist(brandForUpdateDto.Title))
                {                    
                    await _brandRepo.UpdateBrand(brandForUpdateDto);
                }                    
                else
                {
                    throw new Exception("Brand already exists");
                }
            }
        }

        private async Task AddOrRemoveCategoriesOfBrand(BrandDto brand, string[] newCategories, string[] oldCategories)
        {
            var brandId = brand.BrandId;

            var categoriesToAdd = newCategories.Except(oldCategories);

            if (categoriesToAdd.Count() > 0)
                try
                {
                    await _brandRepo.UpdateBrandWithNewCategories(brandId, categoriesToAdd);
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Invalid category", ex.Message);
                }

            var categoriesToRemove = oldCategories.Except(newCategories);

            if (categoriesToRemove.Count() > 0)
                await _brandRepo.RemoveCategoriesFromBrand(brandId, categoriesToRemove);
        }

        public async Task<bool> BrandExist(string title)
        {
            return await _brandRepo.Exists(brand => brand.Title.ToLower().Equals(title.ToLower()));
        }
    }
}