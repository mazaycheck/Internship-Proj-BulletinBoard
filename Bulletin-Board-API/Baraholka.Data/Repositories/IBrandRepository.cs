using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Domain.Models;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<PageDataContainer<BrandDto>> GetPagedBrands(BrandFilterArguments filterOptions, SortingArguments sortingArguments, PageArguments pageArguments);

        Task<BrandDto> GetBrand(int id);

        Task<BrandDto> CreateBrand(BrandDto brandDto);

        Task DeleteBrand(int brandId);

        Task<BrandDto> UpdateBrand(BrandDto brandDto, string[] categories);

        Task<BrandDto> FindBrand(string title);
    }
}