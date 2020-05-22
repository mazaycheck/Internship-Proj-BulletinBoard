using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IBrandCategoryService
    {
        Task<bool> BrandCategoryExists(int brandId, int categoryId);

        Task<bool> DeleteRelation(int brandCategoryId);

        Task<BrandDto> GetBrand(string title);

        Task<CategoryDto> GetCategory(string title);

        Task<List<BrandCategoryDto>> GetAllRelations(string category, string brand);

        Task<BrandCategoryDto> GetRelationById(int id);

        Task<bool> BrandCategoryExists(int brandCategoryId);

        Task<BrandCategoryDto> CreateRelation(int brandId, int categoryId);
    }
}