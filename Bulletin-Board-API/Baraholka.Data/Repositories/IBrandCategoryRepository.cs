using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IBrandCategoryRepository : IGenericRepository<BrandCategory>
    {
        Task<bool> BrandCategoryExists(int brandId, int categoryId);

        Task<BrandCategoryDto> CreateBrandCategory(int brandId, int categoryId);

        Task DeleteBrandCategory(int id);

        Task<List<BrandCategoryDto>> GetAllBrandCategories(string filterCategory, string filterBrand);

        Task<BrandCategoryDto> GetBrandCategory(int brandCategoryId);
    }
}