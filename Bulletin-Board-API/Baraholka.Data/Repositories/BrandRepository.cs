using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BrandRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PageDataContainer<BrandDto>> GetPagedBrands(BrandFilterArguments filterOptions, SortingArguments sortingArguments, PageArguments pageArguments)
        {
            var filters = new List<Expression<Func<Brand, bool>>>()
            {
                brand => brand.Title.Contains(filterOptions.Title ?? ""),
            };

            var includes = new string[] { "BrandCategories.Category" };

            var orderParameters = new List<OrderParams<Brand>>()
            {
                new OrderParams<Brand>{
                    OrderBy = (x) => x.Title,
                    Descending = (sortingArguments.Direction == "desc") }
            };

            IOrderedQueryable<Brand> query = GetAllForPaging(includes, filters, orderParameters);
            PageDataContainer<Brand> pagedBrands = await query.GetPageAsync(pageArguments);
            
            return _mapper.Map<PageDataContainer<BrandDto>>(pagedBrands);
        }

        public async Task<BrandDto> GetBrand(int id)
        {
            var includes = new string[]
            {
                $"{nameof(Brand.BrandCategories)}.{nameof(Category)}"
            };

            var conditions = new List<Expression<Func<Brand, bool>>>
            {
                x => x.BrandId == id
            };
            var brandFromDb = await GetSingle(includes, conditions);

            return _mapper.Map<BrandDto>(brandFromDb);
        }
   
        public async Task<BrandDto> CreateBrand(BrandDto brandDto)
        {
            var brandToCreate = _mapper.Map<Brand>(brandDto);
            var createdBrand = await CreateAndReturn(brandToCreate);
            return _mapper.Map<BrandDto>(createdBrand);
        }

        public async Task DeleteBrand(int brandId)
        {
            await Delete(new Brand { BrandId = brandId });
        }

        public async Task<BrandDto> UpdateBrand(BrandDto brandDto)
        {
            var brandForUpdate = _mapper.Map<Brand>(brandDto);
            var updatedBrand = await UpdateAndReturn(brandForUpdate);

            return _mapper.Map<BrandDto>(updatedBrand);
        }

        public async Task UpdateBrandWithNewCategories(int brandId, IEnumerable<string> categoriesToAdd)
        {
            var categories = categoriesToAdd.Select(x =>
            {
                var category = _context.Categories.Where(p => p.Title == x).FirstOrDefault();
                if (category == null)
                {
                    throw new NullReferenceException($"No such category: {x}");
                }
                return category;
            });

            List<BrandCategory> brandCategoriesToAdd = categories
                .Select(x => new BrandCategory() { BrandId = brandId, CategoryId = x.CategoryId }).ToList();

            _context.BrandCategories.AddRange(brandCategoriesToAdd);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCategoriesFromBrand(int brandId, IEnumerable<string> categoriesToRemove)
        {
            var categories = categoriesToRemove.Select(x => _context.Categories.Where(p => p.Title == x).FirstOrDefault());
 
            foreach (var category in categories)
            {
                var brandcategory = await _context.BrandCategories.Where(p => p.CategoryId == category.CategoryId && p.BrandId == brandId).FirstOrDefaultAsync();
                _context.BrandCategories.Remove(brandcategory);
            }

            await _context.SaveChangesAsync();
        }
    }
}