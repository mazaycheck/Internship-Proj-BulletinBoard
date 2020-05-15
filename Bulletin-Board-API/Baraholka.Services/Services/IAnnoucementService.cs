using Baraholka.Data.Dtos;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IAnnoucementService
    {
        Task<PageDataContainer<AnnoucementViewDto>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
            PageArguments paginateParams, SortingArguments orderParams);

        Task<AnnoucementViewDto> GetAnnoucementForViewById(int id);

        Task<AnnoucementViewDto> CreateAnnoucement(AnnoucementCreateDto annoucementDto, int userId);

        Task<AnnoucementViewDto> UpdateAnnoucement(AnnoucementUpdateDto annoucementDto);

        Task<bool> BrandCategoryExists(int id);

        Task<AnnoucementCheckDto> GetAnnoucementForValidateById(int id);

        Task DeleteAnnoucementById(int id);
    }
}