using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class BrandCategoryService : IBrandCategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IBrandCategoryRepository _brandCategoryRepository;

        public BrandCategoryService(
            IBrandCategoryRepository brandCategoryRepository,
            ICategoryRepository categoryRepo,
            IBrandRepository brandRepo
            )
        {
            _brandCategoryRepository = brandCategoryRepository;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<BrandCategoryDto>> GetAllRelations(string categoryFilter, string brandFilter)
        {
            categoryFilter = categoryFilter?.ToLower() ?? string.Empty;
            brandFilter = brandFilter?.ToLower() ?? string.Empty;

            var allRelations = await _brandCategoryRepository.GetAllBrandCategories(categoryFilter, brandFilter);
            return allRelations;
        }

        public async Task<BrandCategoryDto> GetRelationById(int id)
        {
            BrandCategoryDto brandCategory = await _brandCategoryRepository.GetBrandCategory(id);

            return brandCategory;
        }

        public async Task<BrandCategoryDto> CreateRelation(int brandId, int categoryId)
        {
            BrandCategoryDto brandCatetogyRelation = await _brandCategoryRepository.CreateBrandCategory(brandId, categoryId);

            return brandCatetogyRelation;
        }

        public async Task<bool> DeleteRelation(int brandCategoryId)
        {
            BrandCategoryDto brandCategory = await _brandCategoryRepository.GetBrandCategory(brandCategoryId);

            if (brandCategory == null)
            {
                throw new NullReferenceException($"No brand category relation with id: {brandCategoryId}");
            }

            await _brandCategoryRepository.DeleteBrandCategory(brandCategoryId);

            return true;
        }

        public async Task<CategoryDto> GetCategory(string title)
        {
            var lowerTitle = title.ToLower();
            CategoryDto category = await _categoryRepo.FindCategory(lowerTitle);
            return category;
        }

        public async Task<BrandDto> GetBrand(string title)
        {
            var lowerTitle = title.ToLower();
            BrandDto brand = await _brandRepo.FindBrand(lowerTitle);
            return brand;
        }

        public async Task<bool> BrandCategoryExists(int brandCategoryId)
        {
            return await _brandCategoryRepository.Exists(b => b.BrandCategoryId == brandCategoryId);
        }

        public async Task<bool> BrandCategoryExists(int brandId, int categoryId)
        {
            return await _brandCategoryRepository.BrandCategoryExists(brandId, categoryId);
        }
    }
}