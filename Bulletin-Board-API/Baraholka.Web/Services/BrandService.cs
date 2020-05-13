using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;
using Baraholka.Web.Data.Repositories;
using Baraholka.Web.Helpers;
using Baraholka.Domain.Models;

namespace Baraholka.Web.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;
        private readonly IPageService<Brand> _pageService;

        public BrandService(IBrandRepository brandRepo, IMapper mapper, IPageService<Brand> pageService)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
            _pageService = pageService;
        }

        public async Task<BrandForViewDto> CreateBrand(BrandForCreateDto brandForCreate)
        {
            Brand brand = _mapper.Map<Brand>(brandForCreate);
            await _brandRepo.Create(brand);
            return _mapper.Map<BrandForViewDto>(brand);
        }

        public async Task DeleteBrand(int id)
        {
            var brand = await _brandRepo.GetById(id);
            if (brand == null)
            {
                throw new NullReferenceException($"No such brand with id: {id}");
            }
            await _brandRepo.Delete(brand);
        }

        public async Task<PageDataContainer<BrandForViewDto>> GetAllBrands(BrandFilterArguments filterArguments,
            PageArguments pageArguments, SortingArguments sortingArguments)
        {
            IOrderedQueryable<Brand> brands = _brandRepo.GetBrands(filterArguments, sortingArguments);
            PageDataContainer<Brand> pagedBrands = await _pageService.Paginate(brands, pageArguments);
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
            if (brand == null)
            {
                throw new NullReferenceException($"Not found brand with id: {id}");
            }
            return _mapper.Map<BrandForViewDto>(brand);
        }

        public async Task UpdateBrand(BrandForUpdateDto brandForUpdate)
        {
            Brand brandFromDb = await _brandRepo.GetSingleBrand(brandForUpdate.BrandId);

            if (brandFromDb == null)
            {
                throw new NullReferenceException("No such brand");
            }

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
                    await _brandRepo.UpdateWithNewCategories(brandId, categoriesToAdd);
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Invalid category", ex);
                }

            var categoriesToRemove = oldCategories.Except(newCategories);

            if (categoriesToRemove.Count() > 0)
                await _brandRepo.RemoveCategories(brandId, categoriesToRemove);
        }
    }
}