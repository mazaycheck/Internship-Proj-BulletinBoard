using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class BrandCategoryService : IBrandCategoryService
    {
        private readonly IGenericRepository<BrandCategory> _brandCategoryRepo;
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IGenericRepository<Brand> _brandRepo;
        private readonly IBrandCategoryRepository _brandCategoryRepository;
        private readonly IMapper _mapper;

        public BrandCategoryService(IGenericRepository<BrandCategory> repo, IMapper mapper, IGenericRepository<Category> categoryRepo, IGenericRepository<Brand> brandRepo, IBrandCategoryRepository brandCategoryRepository)
        {
            _brandCategoryRepo = repo;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _brandCategoryRepository = brandCategoryRepository;
            _categoryRepo = categoryRepo;
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
            var brandCatetogyRelation =  await _brandCategoryRepository.CreateBrandCategory(brandId, categoryId);

            return _mapper.Map<BrandCategoryModel>(brandCatetogyRelation);
        }

        public async Task<bool> DeleteRelation(int brandCategoryId)
        {
            BrandCategory brandCategory = await _brandCategoryRepo.FindById(brandCategoryId);

            if (brandCategory == null)
            {
                throw new NullReferenceException($"No brand category relation with id: {brandCategoryId}");
            }

            await _brandCategoryRepo.Delete(brandCategory);

            return true;
        }

        public async Task<CategoryModel> GetCategory(string title)
        {
            var lowerTitle = title.ToLower();
            var category = await _categoryRepo.GetFirst(x => x.Title.ToLower() == lowerTitle);
            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<BrandModel> GetBrand(string title)
        {
            var lowerTitle = title.ToLower();
            var brand = await _brandRepo.GetFirst(x => x.Title.ToLower() == lowerTitle);
            return _mapper.Map<BrandModel>(brand);
        }

        public async Task<bool> BrandCategoryExists(int brandId, int categoryId)
        {
            var filters = new Expression<Func<BrandCategory, bool>>[]
            {
                x => x.CategoryId == categoryId,
                y => y.BrandId == brandId
            };

            return await _brandCategoryRepo.Exists(filters);
        }

        public async Task<bool> BrandCategoryExists(int brandCategoryId)
        {
            return await _brandCategoryRepo.Exists(brandCategoryId);
        }
    }
}