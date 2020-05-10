using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Helpers;

namespace WebApplication2.Services
{
    public interface IAnnoucementService
    {
        Task<PageDataContainer<AnnoucementViewDto>> GetAnnoucements(AnnoucementFilter filterOptions,
            PaginateParams paginateParams, OrderParams orderParams);
        Task<AnnoucementViewDto> GetAnnoucementById(int id);
        Task<AnnoucementViewDto> CreateAnnoucement(AnnoucementCreateDto annoucementDto);
        Task<bool> DeleteAnnoucementById(int id);
        Task<AnnoucementViewDto> UpdateAnnoucement(AnnoucementUpdateDto annoucementDto);
    }
}
