using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Data.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        IOrderedQueryable<Brand> GetBrands(BrandFilterArguments filterOptions, SortingArguments sortingArguments);

        Task<Brand> GetSingleBrand(int id);

        Task UpdateWithNewCategories(int brandId, IEnumerable<string> categoriesToAdd);

        Task RemoveCategories(int brandId, IEnumerable<string> categoriesToRemove);
    }
}