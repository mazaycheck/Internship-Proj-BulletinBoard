using Baraholka.Data.Dtos;
using Baraholka.Services.Models;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IBrandService
    {
        Task<PageDataContainer<BrandModel>> GetAllBrands(BrandFilterArguments filterArguments,
             PageArguments pageArguments, SortingArguments sortingArguments);

        Task<BrandModel> UpdateBrand(BrandUpdateModel brandForUpdate);

        Task DeleteBrand(int brandId);

        Task<BrandModel> GetBrand(int id);

        Task<BrandModel> CreateBrand(BrandCreateModel brand);

        Task<bool> BrandExist(string brand);

        Task<bool> UpdatedBrandExists(BrandUpdateModel brandForUpdate);
    }
}