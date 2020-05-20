using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetCategories(string filter)
        {
            var includes = new string[]
           {
                $"{nameof(Category.BrandCategories)}.{nameof(Brand)}",
           };

            var orderParams = new List<OrderParams<Category>>
            {
                new OrderParams<Category> { OrderBy = (x) => x.Title, Descending = false }
            };

            var lowerFilter = filter?.ToLower() ?? "";
            var filters = new List<Expression<Func<Category, bool>>>
            {
                category => category.Title.ToLower().Contains(lowerFilter)
            };

            List<Category> categories = await GetAll(includes, filters, orderParams);

            return _mapper.Map<List<Category>, List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            var includes = new string[]
            {
                $"{nameof(Category.BrandCategories)}.{nameof(Brand)}",
            };
            var conditions = new List<Expression<Func<Category, bool>>>
            {
                x => x.CategoryId == id
            };

            Category category = await GetSingle(includes, conditions);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            var categoryToCreate = _mapper.Map<Category>(categoryDto);
            var createdCategory = await CreateAndReturn(categoryToCreate);
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<CategoryDto> UpdateCategory(CategoryDto categoryDto)
        {
            var categoryToUpdate = _mapper.Map<Category>(categoryDto);
            var updatedCategory = await UpdateAndReturn(categoryToUpdate);
            return _mapper.Map<CategoryDto>(updatedCategory);
        }

        public async Task DeleteCategory(int categoryId)
        {
            await Delete(new Category { CategoryId = categoryId });
        }
    }
}