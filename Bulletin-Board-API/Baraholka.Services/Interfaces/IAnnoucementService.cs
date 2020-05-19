using Baraholka.Data.Dtos;
using Baraholka.Services.Models;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IAnnoucementService
    {
        Task<PageDataContainer<AnnoucementModel>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
            PageArguments paginateParams, SortingArguments orderParams);

        Task<AnnoucementModel> GetAnnoucement(int id);

        Task<AnnoucementModel> CreateAnnoucement(AnnoucementCreateModel annoucementDto, int userId);

        Task<AnnoucementModel> UpdateAnnoucement(AnnoucementUpdateModel annoucementDto, int userId);

        Task<bool> BrandCategoryExists(int id);

        Task DeleteAnnoucement(int id);
    }
}