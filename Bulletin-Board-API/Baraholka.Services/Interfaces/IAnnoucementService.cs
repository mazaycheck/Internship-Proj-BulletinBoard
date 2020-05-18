using Baraholka.Data.Dtos;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IAnnoucementService
    {
        Task<PageDataContainer<AnnoucementForViewDto>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
            PageArguments paginateParams, SortingArguments orderParams);

        Task<AnnoucementForViewDto> GetAnnoucementForViewById(int id);

        Task<AnnoucementForViewDto> CreateAnnoucement(AnnoucementCreateDto annoucementDto, int userId);

        Task<AnnoucementForViewDto> UpdateAnnoucement(AnnoucementUpdateDto annoucementDto);

        Task<bool> BrandCategoryExists(int id);

        Task<AnnoucementUserInfoDto> GetAnnoucementUserInfo(int id);

        Task DeleteAnnoucementById(int id);
    }
}