using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface ICategoryService
    {
        Task<bool> Exists(string title);

        Task<CategoryDto> CreateCategory(CategoryDto newCategory);

        Task DeleteCategory(int categoryId);

        Task<List<CategoryDto>> GetAllCategories(string filter);

        Task<CategoryDto> GetCategory(int id);

        Task<CategoryDto> UpdateCategory(CategoryDto categoryForUpdate);
    }
}