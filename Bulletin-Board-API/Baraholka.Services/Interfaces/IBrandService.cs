using Baraholka.Data.Dtos;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IBrandService
    {
        Task<PageDataContainer<BrandDto>> GetAllBrands(BrandFilterArguments filterArguments,
             PageArguments pageArguments, SortingArguments sortingArguments);

        Task<BrandDto> UpdateBrand(BrandDto brandForUpdate, string[] categories);

        Task DeleteBrand(int brandId);

        Task<BrandDto> GetBrand(int id);

        Task<BrandDto> CreateBrand(BrandDto brand);

        Task<bool> BrandExist(string brand);

        Task<bool> UpdatedBrandExists(BrandDto brand);
    }
}