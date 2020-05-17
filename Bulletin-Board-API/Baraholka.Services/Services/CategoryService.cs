using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> repository, IMapper mapper)
        {
            _categoryRepository = repository;
            _mapper = mapper;
        }

        public async Task<List<CategoryForViewDto>> GetAllCategories(string filter)
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

            List<Category> categories = await _categoryRepository.GetAll(includes, filters, orderParams);

            if (categories.Count == 0)
            {
                return null;
            }

            return _mapper.Map<List<Category>, List<CategoryForViewDto>>(categories);
        }

        public async Task<CategoryForViewDto> GetCategory(int id)
        {
            var includes = new string[]
            {
                $"{nameof(Category.BrandCategories)}.{nameof(Brand)}",
            };
            var conditions = new List<Expression<Func<Category, bool>>>
            {
                x => x.CategoryId == id
            };

            Category category = await _categoryRepository.GetSingle(includes, conditions);

            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryForViewDto>(category);
        }

        public async Task<CategoryForViewDto> CreateCategory(CategoryForCreateDto newCategory)
        {
            Category category = _mapper.Map<Category>(newCategory);
            await _categoryRepository.Create(category);
            return _mapper.Map<CategoryForViewDto>(category);
        }

        public async Task<CategoryForViewDto> UpdateCategory(CategoryForUpdateDto categoryForUpdate)
        {
            var categoryFromDb = await _categoryRepository.GetSingle(x => x.CategoryId == categoryForUpdate.CategoryId);
            if (categoryFromDb.Title != categoryForUpdate.Title && await Exists(categoryForUpdate.Title))
            {
                throw new Exception("Duplicate title");
            }

            Category category = _mapper.Map<Category>(categoryForUpdate);
            await _categoryRepository.Update(category);
            return _mapper.Map<CategoryForViewDto>(category);
        }

        public async Task DeleteCategory(CategoryBasicDto categoryDto)
        {
            var categoryToDelete = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.Delete(categoryToDelete);
        }

        public async Task<bool> Exists(string title)
        {
            return await _categoryRepository.Exists(category => category.Title.ToLower().Contains(title.ToLower()));
        }

        public async Task<CategoryBasicDto> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetSingle(x => x.CategoryId ==  id);
            return _mapper.Map<CategoryBasicDto>(category);
        }
    }
}