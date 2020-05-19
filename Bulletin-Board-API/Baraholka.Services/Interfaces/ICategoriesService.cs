using Baraholka.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface ICategoryService
    {
        Task<bool> Exists(string title);

        Task<CategoryModel> CreateCategory(CategoryCreateModel newCategory);

        Task DeleteCategory(int categoryId);

        Task<List<CategoryModel>> GetAllCategories(string filter);

        Task<CategoryModel> GetCategory(int id);

        Task<CategoryModel> UpdateCategory(CategoryUpdateModel categoryForUpdate);
    }
}