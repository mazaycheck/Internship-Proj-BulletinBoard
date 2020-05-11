using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Data.Repositories
{
    public interface IAnnoucementRepository : IGenericRepository<Annoucement>
    {
        Task<PageDataContainer<Annoucement>> GetPagedAnnoucements(AnnoucementFilterArguments filterOptions,
                     PageArguments paginateParams, SortingArguments orderParams);

        Task<Annoucement> GetAnnoucementById(int id);
    }
}