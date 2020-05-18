using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface ITownService
    {
        Task<TownServiceDto> CreateTown(TownForCreateDto townDto);

        Task DeleteTown(TownServiceDto townDto);

        Task<bool> Exists(string title);

        Task<TownServiceDto> FindTown(int id);

        Task<PageDataContainer<TownServiceDto>> GetTownsForAdmin(string filter, PageArguments pageArguments);

        Task<List<TownForPublicViewDto>> GetTownsForPublic();

        Task<TownServiceDto> UpdateTown(TownForUpdateDto townDto);
    }
}