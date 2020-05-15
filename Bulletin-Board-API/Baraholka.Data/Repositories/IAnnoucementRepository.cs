using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IAnnoucementRepository : IGenericRepository<Annoucement>
    {
        Task<Annoucement> GetSingleAnnoucementForViewById(int id);

        Task<PageDataContainer<Annoucement>> GetPagedAnnoucements(AnnoucementFilterArguments filterOptions, PageArguments paginateParams, SortingArguments orderParams);
        Task BindImages(Annoucement annoucement, List<string> fileGuidNames);
    }
}