using Baraholka.Data.Dtos;
using System.Threading.Tasks;

namespace Baraholka.Web.Services
{
    public interface IAnnoucementService
    {
        Task<PageDataContainer<AnnoucementViewDto>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
            PageArguments paginateParams, SortingArguments orderParams);

        Task<AnnoucementViewDto> GetAnnoucementById(int id);

        Task<AnnoucementViewDto> CreateAnnoucement(AnnoucementCreateDto annoucementDto);

        Task<bool> DeleteAnnoucementById(int id);

        Task<AnnoucementViewDto> UpdateAnnoucement(AnnoucementUpdateDto annoucementDto);
    }
}