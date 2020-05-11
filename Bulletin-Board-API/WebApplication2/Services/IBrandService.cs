using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Helpers;

namespace WebApplication2.Services
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