using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public interface IAnnoucementService
    {
        Task<Paged<AnnoucementForViewDto>> GetAnnoucements(AnnoucementFilter filterOptions,
            PaginateParams paginateParams, OrderParams orderParams);
        Task<AnnoucementForViewDto> GetAnnoucementById(int id);
        Task<AnnoucementForViewDto> CreateNewAnnoucement(AnnoucementForCreateDto annoucementDto, int userId);
        Task<bool> DeleteAnnoucementById(int id);
        Task<AnnoucementForViewDto> UpdateAnnoucement(AnnoucementForUpdateDto annoucementDto, int userId);
    }
}
