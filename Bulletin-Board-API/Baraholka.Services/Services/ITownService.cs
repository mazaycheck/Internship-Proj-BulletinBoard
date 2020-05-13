using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface ITownService
    {
        Task<TownForAdminViewDto> CreateTown(TownForCreateDto townDto);

        Task DeleteTown(int id);

        Task<TownForAdminViewDto> GetTownForAdmin(int id);

        Task<PageDataContainer<TownForAdminViewDto>> GetTownsForAdmin(string filter, PageArguments pageArguments);

        Task<List<TownForPublicViewDto>> GetTownsForPublic();

        Task<TownForAdminViewDto> UpdateTown(TownForUpdateDto townDto);
    }
}