using Baraholka.Data.Dtos;
using System.Threading.Tasks;
using Baraholka.Services.Models;

namespace Baraholka.Services
{
    public interface IBrandService
    {
        Task<PageDataContainer<BrandModel>> GetAllBrands(BrandFilterArguments filterArguments,
             PageArguments pageArguments, SortingArguments sortingArguments);

        Task<BrandDto> UpdateBrand(BrandUpdateModel brandForUpdate);

        Task DeleteBrand(int brandId);

        Task<BrandModel> GetBrand(int id);

        Task<BrandModel> CreateBrand(BrandCreateModel brand);

        Task<bool> BrandExist(string brand);
    }
}