using Baraholka.Data.Dtos;
using Baraholka.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public interface ITownService
    {
        Task<TownModel> CreateTown(TownCreateModel townDto);

        Task DeleteTown(int townId);

        Task<bool> Exists(string title);

        Task<TownDto> FindTown(string title);

        Task<PageDataContainer<TownModel>> GetPagedTowns(string filter, PageArguments pageArguments);

        Task<List<TownModel>> GetAllTowns();

        Task<TownModel> UpdateTown(TownUpdateModel townDto);

        Task<TownModel> GetTown(int id);
    }
}