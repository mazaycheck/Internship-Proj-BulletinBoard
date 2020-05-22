using Baraholka.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IBrandCategoryService
    {
        Task<bool> BrandCategoryExists(int brandId, int categoryId);

        Task<bool> DeleteRelation(int brandCategoryId);

        Task<BrandModel> GetBrand(string title);

        Task<CategoryModel> GetCategory(string title);

        Task<List<BrandCategoryModel>> GetAllRelations(string category, string brand);

        Task<BrandCategoryModel> GetRelationById(int id);

        Task<bool> BrandCategoryExists(int brandCategoryId);

        Task<BrandCategoryModel> CreateRelation(int brandId, int categoryId);
    }
}