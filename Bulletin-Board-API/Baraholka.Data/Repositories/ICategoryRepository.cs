using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<CategoryDto> CreateCategory(CategoryDto categoryDto);

        Task DeleteCategory(int categoryId);

        Task<CategoryDto> FindCategory(string title);

        Task<List<CategoryDto>> GetCategories(string filter);

        Task<CategoryDto> GetCategory(int id);

        Task<CategoryDto> UpdateCategory(CategoryDto categoryDto);
    }
}