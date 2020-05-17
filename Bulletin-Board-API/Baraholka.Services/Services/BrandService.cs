using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using System;
using System.Linq;
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

        public async Task<PageDataContainer<BrandForViewDto>> GetAllBrands(BrandFilterArguments filterArguments,
            PageArguments pageArguments, SortingArguments sortingArguments)
        {
            PageDataContainer<Brand> pagedBrands = await _brandRepo.GetPagedBrands(filterArguments, sortingArguments, pageArguments);

            if (pagedBrands.PageData.Count > 0)
            {
                PageDataContainer<BrandForViewDto> pagedBrandDtos = _mapper.Map<PageDataContainer<BrandForViewDto>>(pagedBrands);
                return pagedBrandDtos;
            }

            return null;
        }

        public async Task<BrandForViewDto> GetBrand(int id)
        {
            Brand brand = await _brandRepo.GetSingleBrand(id);
            return _mapper.Map<BrandForViewDto>(brand);
        }

        public async Task<BrandForViewDto> CreateBrand(BrandForCreateDto brandForCreate)
        {
            Brand brand = _mapper.Map<Brand>(brandForCreate);
            await _brandRepo.Create(brand);
            return _mapper.Map<BrandForViewDto>(brand);
        }

        public async Task DeleteBrand(BrandForViewDto brand)
        {
            await _brandRepo.Delete(_mapper.Map<Brand>(brand));
        }

        public async Task UpdateBrand(BrandForUpdateDto brandForUpdate)
        {
            Brand brandFromDb = await _brandRepo.GetSingleBrand(brandForUpdate.BrandId);

            if (brandFromDb.Title != brandForUpdate.Title)
            {
                brandFromDb.Title = brandForUpdate.Title;
                await _brandRepo.Save();
            }

            string[] categoriesOfBrandFromDb = brandFromDb.BrandCategories.Select(x => x.Category.Title).ToArray();

            if (!categoriesOfBrandFromDb.SequenceEqual(brandForUpdate.Categories))
            {
                await AddOrRemoveCategoriesOfBrand(brandFromDb, brandForUpdate.Categories, categoriesOfBrandFromDb);
            }
        }

        private async Task AddOrRemoveCategoriesOfBrand(Brand brand, string[] newCategories, string[] oldCategories)
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
            return await _brandRepo.Exists(brand => brand.Title.ToLower().Contains(title.ToLower()));
        }
    }
}