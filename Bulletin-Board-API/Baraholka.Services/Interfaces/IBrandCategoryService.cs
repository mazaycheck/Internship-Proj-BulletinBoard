using Baraholka.Data.Dtos;
using Baraholka.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IBrandCategoryService
    {
        Task<bool> BrandCategoryExists(int brandId, int categoryId);

        Task<bool> DeleteRelation(int brandCategoryId);

        Task<BrandForViewDto> GetBrand(string title);

        Task<CategoryModel> GetCategory(string title);

        Task<List<BrandCategoryForViewDto>> GetAllRelations(string category, string brand);

        Task<BrandCategoryForViewDto> GetRelationById(int id);

        Task<bool> BrandCategoryExists(int brandCategoryId);

        Task<BrandCategoryForViewDto> CreateRelation(int brandId, int categoryId);
    }
}