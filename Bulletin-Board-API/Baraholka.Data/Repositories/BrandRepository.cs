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

        public async Task<BrandDto> FindBrand(string title)
        {
            var brand = await GetSingle(x => x.Title == title);
            return _mapper.Map<BrandDto>(brand);
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

        public async Task<BrandDto> UpdateBrand(BrandDto brandDto, string[] categories)
        {
            var updatedBrand = await UpdateBrandProperties(brandDto);

            updatedBrand = _mapper.Map<BrandDto>(await UpdateAssociatedCategories(brandDto.BrandId, categories));

            return _mapper.Map<BrandDto>(updatedBrand);
        }

        public async Task<BrandDto> UpdateBrandProperties(BrandDto brandDto)
        {
            var brandForUpdate = _mapper.Map<Brand>(brandDto);
            var updatedBrand = await UpdateAndReturn(brandForUpdate);

            return _mapper.Map<BrandDto>(updatedBrand);
        }

        private async Task<Brand> UpdateAssociatedCategories(int brandId, string[] newCategories)
        {
            var brandFromDb = await GetBrand(brandId);

            string[] oldCategories = brandFromDb.BrandCategories.Select(x => x.Category.Title).ToArray();

            if (!oldCategories.SequenceEqual(newCategories))
            {
                await ComputeCategoryDifferences(brandFromDb.BrandId, newCategories, oldCategories);
            }
            return await FindById(brandFromDb.BrandId);
        }

        private async Task ComputeCategoryDifferences(int brandId, string[] newCategories, string[] oldCategories)
        {
            var categoriesToAdd = newCategories.Except(oldCategories);

            if (categoriesToAdd.Count() > 0)
                try
                {
                    await UpdateBrandWithNewCategories(brandId, categoriesToAdd);
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Invalid category", ex.Message);
                }

            var categoriesToRemove = oldCategories.Except(newCategories);

            if (categoriesToRemove.Count() > 0)
                await RemoveCategoriesFromBrand(brandId, categoriesToRemove);
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