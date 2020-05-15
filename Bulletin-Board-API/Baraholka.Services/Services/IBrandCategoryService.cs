using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IBrandCategoryService
    {
        Task<bool> BrandCategoryExist(int brandId, int categoryId);

        Task<BrandCategoryForViewDto> CreateRelation(BrandCategoryForCreateDto brandCategoryForCreate);

        Task<bool> DeleteRelation(int brandCategoryId);

        Task<BrandForViewDto> GetBrand(string title);

        Task<CategoryForViewDto> GetCategory(string title);

        Task<List<BrandCategoryForViewDto>> GetAllRelations(string category, string brand);

        Task<BrandCategoryForViewDto> GetRelationById(int id);
        Task<bool> BrandCategoryExist(int brandCategoryId);
    }
}