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
        private readonly IMapper _mapper;

        public BrandCategoryService(IGenericRepository<BrandCategory> repo, IMapper mapper, IGenericRepository<Category> categoryRepo, IGenericRepository<Brand> brandRepo)
        {
            _brandCategoryRepo = repo;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<BrandCategoryModel>> GetAllRelations(string categoryTitle, string brandTitle)
        {
            var includes = new List<Expression<Func<BrandCategory, object>>>()
            {
                x => x.Category,
                y => y.Brand
            };

            var filters = new List<Expression<Func<BrandCategory, bool>>>()
            {
                x => x.Category.Title.ToLower().Contains(categoryTitle.ToLower() ?? ""),
                y => y.Brand.Title.ToLower().Contains(brandTitle.ToLower() ?? "")
            };

            var orderParams = new List<OrderParams<BrandCategory>>
            {
                new OrderParams<BrandCategory>{ OrderBy = (x) => x.Category.Title},
                new OrderParams<BrandCategory>{ OrderBy = (x) => x.Brand.Title}
            };

            List<BrandCategory> brandCategories = await _brandCategoryRepo.GetAll(includes, filters, orderParams);
            return _mapper.Map<List<BrandCategory>, List<BrandCategoryModel>>(brandCategories);
        }

        public async Task<BrandCategoryModel> GetRelationById(int id)
        {
            var includes = new List<Expression<Func<BrandCategory, object>>>
            {
                p => p.Brand,
                k => k.Category
            };

            BrandCategory brandCategory = await _brandCategoryRepo.FindById(id, includes, null);

            return _mapper.Map<BrandCategoryModel>(brandCategory);
        }

        public async Task<BrandCategoryModel> CreateRelation(int brandId, int categoryId)
        {
            var brandCatetogyRelation = new BrandCategory() { BrandId = brandId, CategoryId = categoryId };
            await _brandCategoryRepo.Create(brandCatetogyRelation);

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