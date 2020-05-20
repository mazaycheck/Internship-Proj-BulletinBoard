using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Services.Models;
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

        public async Task<List<CategoryModel>> GetAllCategories(string filter)
        {
            var categories = await _categoryRepository.GetCategories(filter);
            if (categories.Count == 0)
            {
                return null;
            }

            return _mapper.Map<List<CategoryDto>, List<CategoryModel>>(categories);
        }

        public async Task<CategoryModel> GetCategory(int id)
        {
            CategoryDto category = await _categoryRepository.GetCategory(id);

            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel> CreateCategory(CategoryCreateModel categoryCreateModel)
        {
            CategoryDto category = _mapper.Map<CategoryDto>(categoryCreateModel);
            CategoryDto newCategory = await _categoryRepository.CreateCategory(category);
            return _mapper.Map<CategoryModel>(newCategory);
        }

        public async Task<CategoryModel> UpdateCategory(CategoryUpdateModel categoryUpdateModel)
        {
            CategoryDto category = _mapper.Map<CategoryDto>(categoryUpdateModel);
            await _categoryRepository.UpdateCategory(category);
            return _mapper.Map<CategoryModel>(category);
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