using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<BrandCategoryForViewDto>> GetAll(string categoryTitle, string brandTitle)
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
            var brandCategories = await _brandCategoryRepo.GetAllIncludeFilter(includes, filters);

            var dataForView = brandCategories.Select(x => _mapper.Map<BrandCategoryForViewDto>(x)).ToList();

            return dataForView;
        }

        public async Task<BrandCategoryForViewDto> GetById(int id)
        {
            var navigationProperties = new List<Expression<Func<BrandCategory, object>>>
            {
                p => p.Brand,
                k => k.Category
            };
            BrandCategory brandCategory = await _brandCategoryRepo.GetByIdInclude(id, navigationProperties);

            return _mapper.Map<BrandCategoryForViewDto>(brandCategory);
        }

        public async Task<BrandCategoryForViewDto> CreateRelation(BrandCategoryForCreateDto brandCategoryForCreate)
        {
            string brandTitle = brandCategoryForCreate.Brand;
            string categoryTitle = brandCategoryForCreate.Category;
            var brandFromDb = (await _brandRepo.FindFirst(x => x.Title == brandTitle));
            var categoryFromDb = (await _categoryRepo.FindFirst(x => x.Title == categoryTitle));

            if (brandFromDb == null || categoryFromDb == null)
            {
                throw new NullReferenceException("No such brand or category");
            }

            var brandCat = new BrandCategory() { Brand = brandFromDb, Category = categoryFromDb };
            bool found = await _brandCategoryRepo.Exists(x => x == brandCat);

            if (found)
            {
                throw new ArgumentException($"This relation already exists: {brandTitle}, {categoryTitle}");
            }

            await _brandCategoryRepo.Create(brandCat);

            return _mapper.Map<BrandCategoryForViewDto>(brandCat);

        }

        public async Task<bool> DeleteRelation(int brandCategoryId)
        {
            var brandCategory = await _brandCategoryRepo.GetById(brandCategoryId);
            if (brandCategory == null)
            {
                throw new NullReferenceException($"No brand category relation with id: {brandCategoryId}");
            }
            await _brandCategoryRepo.Delete(brandCategory);
            return true;
        }
    }
}
