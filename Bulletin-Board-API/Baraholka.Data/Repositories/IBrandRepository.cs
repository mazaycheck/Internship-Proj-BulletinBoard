using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<PageDataContainer<Brand>> GetPagedBrands(BrandFilterArguments filterOptions, SortingArguments sortingArguments, PageArguments pageArguments);

        Task<Brand> GetSingleBrand(int id);

        Task UpdateBrandWithNewCategories(int brandId, IEnumerable<string> categoriesToAdd);

        Task RemoveCategoriesFromBrand(int brandId, IEnumerable<string> categoriesToRemove);
    }
}