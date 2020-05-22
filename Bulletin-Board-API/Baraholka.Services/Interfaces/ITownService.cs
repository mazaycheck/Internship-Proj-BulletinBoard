using Baraholka.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface ITownService
    {
        Task<TownDto> CreateTown(TownDto townDto);

        Task DeleteTown(int townId);

        Task<bool> Exists(string title);

        Task<TownDto> FindTown(string title);

        Task<PageDataContainer<TownDto>> GetPagedTowns(string filter, PageArguments pageArguments);

        Task<List<TownDto>> GetAllTowns();

        Task<TownDto> UpdateTown(TownDto townDto);

        Task<TownDto> GetTown(int id);
    }
}