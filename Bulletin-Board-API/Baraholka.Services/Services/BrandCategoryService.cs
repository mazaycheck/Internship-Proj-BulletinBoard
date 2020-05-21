using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Services.Models;
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
        private readonly IMapper _mapper;

        public BrandCategoryService(
            IBrandCategoryRepository brandCategoryRepository,
            IMapper mapper,
            ICategoryRepository categoryRepo,
            IBrandRepository brandRepo
            )
        {
            _brandCategoryRepository = brandCategoryRepository;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<List<BrandCategoryModel>> GetAllRelations(string categoryFilter, string brandFilter)
        {
            categoryFilter = categoryFilter?.ToLower() ?? string.Empty;
            brandFilter = brandFilter?.ToLower() ?? string.Empty;

            var allRelations = await _brandCategoryRepository.GetAllBrandCategories(categoryFilter, brandFilter);
            return _mapper.Map<List<BrandCategoryDto>, List<BrandCategoryModel>>(allRelations);
        }

        public async Task<BrandCategoryModel> GetRelationById(int id)
        {
            BrandCategoryDto brandCategory = await _brandCategoryRepository.GetBrandCategory(id);

            return _mapper.Map<BrandCategoryModel>(brandCategory);
        }

        public async Task<BrandCategoryModel> CreateRelation(int brandId, int categoryId)
        {
            var brandCatetogyRelation = await _brandCategoryRepository.CreateBrandCategory(brandId, categoryId);

            return _mapper.Map<BrandCategoryModel>(brandCatetogyRelation);
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

        public async Task<CategoryModel> GetCategory(string title)
        {
            var lowerTitle = title.ToLower();
            var category = await _categoryRepo.FindCategory(lowerTitle);
            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<BrandModel> GetBrand(string title)
        {
            var lowerTitle = title.ToLower();
            var brand = await _brandRepo.FindBrand(lowerTitle);
            return _mapper.Map<BrandModel>(brand);
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