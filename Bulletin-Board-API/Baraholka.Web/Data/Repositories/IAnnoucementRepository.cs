using System.Linq;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;
using Baraholka.Domain.Models;

namespace Baraholka.Web.Data.Repositories
{
    public interface IAnnoucementRepository : IGenericRepository<Annoucement>
    {
        Task<Annoucement> GetAnnoucementById(int id);

        IOrderedQueryable<Annoucement> GetAnnoucementsForPaging(AnnoucementFilterArguments filterOptions, PageArguments paginateParams, SortingArguments orderParams);
    }
}