using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface ICategoryService
    {
        Task<bool> Exists(string title);

        Task<CategoryForViewDto> CreateCategory(CategoryForCreateDto newCategory);

        Task DeleteCategory(CategoryBasicDto category);

        Task<List<CategoryForViewDto>> GetAllCategories(string filter);

        Task<CategoryForViewDto> GetCategory(int id);

        Task<CategoryForViewDto> UpdateCategory(CategoryForUpdateDto categoryForUpdate);

        Task<CategoryBasicDto> GetCategoryById(int id);
    }
}