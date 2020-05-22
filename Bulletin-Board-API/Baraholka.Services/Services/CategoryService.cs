using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllCategories(string filter)
        {
            List<CategoryDto> categories = await _categoryRepository.GetCategories(filter);
            if (categories.Count == 0)
            {
                return null;
            }

            return categories;
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            CategoryDto category = await _categoryRepository.GetCategory(id);

            if (category == null)
            {
                return null;
            }

            return category;
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            CategoryDto newCategory = await _categoryRepository.CreateCategory(categoryDto);
            return newCategory;
        }

        public async Task<CategoryDto> UpdateCategory(CategoryDto categoryUpdate)
        {
            CategoryDto updatedCategory = await _categoryRepository.UpdateCategory(categoryUpdate);
            return updatedCategory;
        }

        public async Task DeleteCategory(int categoryId)
        {
            await _categoryRepository.DeleteCategory(categoryId);
        }

        public async Task<bool> Exists(string title)
        {
            return await _categoryRepository.Exists(category => category.Title.ToLower().Contains(title.ToLower()));
        }
    }
}