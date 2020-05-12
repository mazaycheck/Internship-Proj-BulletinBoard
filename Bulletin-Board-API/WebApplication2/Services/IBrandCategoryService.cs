using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;

namespace WebApplication2.Services
{
    public interface IBrandCategoryService
    {
        Task<BrandCategoryForViewDto> CreateRelation(BrandCategoryForCreateDto brandCategoryForCreate);

        Task<bool> DeleteRelation(int brandCategoryId);

        Task<List<BrandCategoryForViewDto>> GetAllRelations(string category, string brand);

        Task<BrandCategoryForViewDto> GetRelationById(int id);
    }
}