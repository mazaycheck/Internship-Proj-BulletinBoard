using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;

namespace WebApplication2.Services
{
    public interface ICategoryService
    {
        Task<CategoryForViewDto> CreateCategory(CategoryForCreateDto newCategory);
        Task DeleteCategory(int id);
        Task<List<CategoryForViewDto>> GetAllCategories(string filter);
        Task<CategoryForViewDto> GetCategory(int id);
    }
}