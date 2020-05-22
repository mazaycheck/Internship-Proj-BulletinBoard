using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public interface ITownRepository : IGenericRepository<Town>
    {
        Task<TownDto> CreateTown(TownDto townDto);

        Task DeleteTown(int townId);

        Task<TownDto> FindTown(string title);

        Task<List<TownDto>> GetAllTowns();

        Task<PageDataContainer<TownDto>> GetPagedTows(string filter, PageArguments pageArguments);

        Task<TownDto> GetTown(int id);

        Task<bool> TownExists(string title);

        Task<TownDto> UpdateTown(TownDto townDto);
    }
}