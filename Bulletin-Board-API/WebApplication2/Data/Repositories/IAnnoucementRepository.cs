using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Data.Repositories
{
    public interface IAnnoucementRepository : IGenericRepository<Annoucement>
    {
        Task<Annoucement> GetAnnoucementById(int id);

        IOrderedQueryable<Annoucement> GetAnnoucementsForPaging(AnnoucementFilterArguments filterOptions, PageArguments paginateParams, SortingArguments orderParams);
    }
}