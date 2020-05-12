using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;

namespace WebApplication2.Services
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

        public async Task<List<BrandCategoryForViewDto>> GetAllRelations(string categoryTitle, string brandTitle)
        {
            var includes = new List<Expression<Func<BrandCategory, object>>>()
            {
                x => x.Category,
                y => y.Brand
            };

            var filters = new List<Expression<Func<BrandCategory, bool>>>()
            {
                x => x.Category.Title.Contains(categoryTitle ?? ""),
                y => y.Brand.Title.Contains(brandTitle ?? "")
            };

            var orderParams = new List<OrderParams<BrandCategory>>
            {
                new OrderParams<BrandCategory>{ OrderBy = (x) => x.Category.Title},
                new OrderParams<BrandCategory>{ OrderBy = (x) => x.Brand.Title}
            };

            List<BrandCategory> brandCategories = await _brandCategoryRepo.GetAll(includes, filters, orderParams);
            return _mapper.Map<List<BrandCategory>, List<BrandCategoryForViewDto>>(brandCategories);
        }

        public async Task<BrandCategoryForViewDto> GetRelationById(int id)
        {
            var includes = new List<Expression<Func<BrandCategory, object>>>
            {
                p => p.Brand,
                k => k.Category
            };

            BrandCategory brandCategory = await _brandCategoryRepo.GetById(id, includes, null);

            return _mapper.Map<BrandCategoryForViewDto>(brandCategory);
        }

        public async Task<BrandCategoryForViewDto> CreateRelation(BrandCategoryForCreateDto brandCategoryForCreate)
        {
            string brandTitle = brandCategoryForCreate.Brand;
            string categoryTitle = brandCategoryForCreate.Category;

            Category categoryFromDb = await _categoryRepo.GetSingle(x => x.Title == categoryTitle)
                ?? throw new NullReferenceException("No such category");

            Brand brandFromDb = await _brandRepo.GetSingle(x => x.Title == brandTitle)
                ?? throw new NullReferenceException("No such brand");

            var filters = new Expression<Func<BrandCategory, bool>>[]
            {
                x => x.CategoryId == categoryFromDb.CategoryId,
                y => y.BrandId == brandFromDb.BrandId
            };

            if (await _brandCategoryRepo.Exists(filters))
                throw new ArgumentException($"This relation already exists: {categoryTitle} / {brandTitle}");

            var brandCatetogyRelation = new BrandCategory() { BrandId = brandFromDb.BrandId, CategoryId = categoryFromDb.CategoryId };
            await _brandCategoryRepo.Create(brandCatetogyRelation);

            return _mapper.Map<BrandCategoryForViewDto>(brandCatetogyRelation);
        }

        public async Task<bool> DeleteRelation(int brandCategoryId)
        {
            BrandCategory brandCategory = await _brandCategoryRepo.GetById(brandCategoryId);

            if (brandCategory == null)
            {
                throw new NullReferenceException($"No brand category relation with id: {brandCategoryId}");
            }

            await _brandCategoryRepo.Delete(brandCategory);

            return true;
        }
    }
}