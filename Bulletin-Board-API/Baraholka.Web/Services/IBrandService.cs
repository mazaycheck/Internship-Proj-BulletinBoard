using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;

namespace Baraholka.Web.Services
{
    public interface IBrandService
    {
        Task<PageDataContainer<BrandForViewDto>> GetAllBrands(BrandFilterArguments filterArguments,
             PageArguments pageArguments, SortingArguments sortingArguments);

        Task UpdateBrand(BrandForUpdateDto brandForUpdate);

        Task DeleteBrand(int id);

        Task<BrandForViewDto> GetBrand(int id);

        Task<BrandForViewDto> CreateBrand(BrandForCreateDto brand);
    }
}