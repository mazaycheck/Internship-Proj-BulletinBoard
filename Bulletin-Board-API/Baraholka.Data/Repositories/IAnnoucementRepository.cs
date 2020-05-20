using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface IAnnoucementRepository : IGenericRepository<Annoucement>
    {
        Task<PageDataContainer<AnnoucementDto>> GetPagedAnnoucements(AnnoucementFilterArguments filterOptions, PageArguments paginateParams, SortingArguments orderParams);

        Task SaveImageFileNames(int annoucementId, List<string> fileGuidNames);

        Task<AnnoucementDto> GetSingleAnnoucementForView(int id);

        Task<AnnoucementDto> CreateAnnoucement(AnnoucementDto annoucementDto);

        Task<AnnoucementDto> UpdateAnnoucement(AnnoucementDto annoucementDto);

        Task<AnnoucementDto> GetSingleAnnoucement(int id);
    }
}