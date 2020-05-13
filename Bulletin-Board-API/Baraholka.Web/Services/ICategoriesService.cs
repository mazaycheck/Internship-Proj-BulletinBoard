using System.Collections.Generic;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;

namespace Baraholka.Web.Services
{
    public interface ICategoryService
    {
        Task<CategoryForViewDto> CreateCategory(CategoryForCreateDto newCategory);

        Task DeleteCategory(int id);

        Task<List<CategoryForViewDto>> GetAllCategories(string filter);

        Task<CategoryForViewDto> GetCategory(int id);
    }
}