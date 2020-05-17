﻿using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
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

        public BrandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PageDataContainer<Brand>> GetPagedBrands(BrandFilterArguments filterOptions, SortingArguments sortingArguments, PageArguments pageArguments)
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

            return pagedBrands;
        }

        public async Task<Brand> GetSingleBrand(int id)
        {
            var includes = new string[]
            {
                $"{nameof(Brand.BrandCategories)}.{nameof(Category)}"
            };

            var conditions = new List<Expression<Func<Brand, bool>>>
            {
                x => x.BrandId == id
            };

            return await GetSingle(includes, conditions);
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

            List<BrandCategory> brandCategoriesToRemove = categories
                .Select(x => _context.BrandCategories.Where(p => p.CategoryId == x.CategoryId && p.BrandId == brandId).FirstOrDefault()).ToList();

            _context.BrandCategories.RemoveRange(brandCategoriesToRemove);

            await _context.SaveChangesAsync();
        }
    }
}