using Baraholka.Data.Dtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface IAnnoucementService
    {
        Task<PageDataContainer<AnnoucementDto>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
            PageArguments paginateParams, SortingArguments orderParams);

        Task<AnnoucementDto> GetAnnoucement(int id);

        Task<AnnoucementDto> CreateAnnoucement(AnnoucementDto annoucementDto, List<IFormFile> images);

        Task<AnnoucementDto> UpdateAnnoucement(AnnoucementDto annoucementDto, List<IFormFile> images);

        Task<bool> BrandCategoryExists(int id);

        Task DeleteAnnoucement(int id);
    }
}