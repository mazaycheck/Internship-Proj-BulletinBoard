using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public class BrandCategoryRepository : GenericRepository<BrandCategory>, IBrandCategoryRepository
    {
        private readonly IMapper _mapper;

        public BrandCategoryRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<List<BrandCategoryDto>> GetAllBrandCategories(string filterCategory, string filterBrand)
        {
            var includes = new List<Expression<Func<BrandCategory, object>>>()
            {
                x => x.Category,
                y => y.Brand
            };

            var filters = new List<Expression<Func<BrandCategory, bool>>>()
            {
                x => x.Category.Title.ToLower().Contains(filterCategory),
                y => y.Brand.Title.ToLower().Contains(filterBrand)
            };

            var orderParams = new List<OrderParams<BrandCategory>>
            {
                new OrderParams<BrandCategory>{ OrderBy = (x) => x.Category.Title},
                new OrderParams<BrandCategory>{ OrderBy = (x) => x.Brand.Title}
            };

            List<BrandCategory> brandCategories = await GetAll(includes, filters, orderParams);

            return _mapper.Map<List<BrandCategory>, List<BrandCategoryDto>>(brandCategories);
        }

        public async Task<BrandCategoryDto> GetBrandCategory(int brandCategoryId)
        {
            var includes = new string[]
            {
                $"{nameof(Brand)}",
                $"{nameof(Category)}",
            };

            var filters = new List<Expression<Func<BrandCategory, bool>>>()
            {
                b => b.BrandCategoryId == brandCategoryId,
                
            };

            var brandCategory = await GetSingle(includes, filters);

            return _mapper.Map<BrandCategoryDto>(brandCategory);
        }

        public async Task<BrandCategoryDto> CreateBrandCategory(int brandId, int categoryId)
        {
            var brandCatetogyRelation = new BrandCategory() { BrandId = brandId, CategoryId = categoryId };
            var newBrandCategory = await CreateAndReturn(brandCatetogyRelation);
            return _mapper.Map<BrandCategoryDto>(newBrandCategory);
        }
    }
}