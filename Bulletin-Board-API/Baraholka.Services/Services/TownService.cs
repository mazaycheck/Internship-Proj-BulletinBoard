using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class TownService : ITownService
    {
        private readonly ITownRepository _townRepository;

        public TownService(ITownRepository townRepository)
        {
            _townRepository = townRepository;
        }

        public async Task<TownDto> GetTown(int id)
        {
            return await _townRepository.GetTown(id);
        }

        public async Task<List<TownDto>> GetAllTowns()
        {
            List<TownDto> towns = await _townRepository.GetAllTowns();

            return towns;
        }

        public async Task<PageDataContainer<TownDto>> GetPagedTowns(string filter, PageArguments pageArguments)
        {
            filter = filter?.ToLower() ?? string.Empty;
            PageDataContainer<TownDto> pagedTowns = await _townRepository.GetPagedTows(filter, pageArguments);

            if (pagedTowns.PageData.Count == 0)
            {
                return null;
            }

            return pagedTowns;
        }

        public async Task<TownDto> CreateTown(TownDto townDto)
        {
            var newTown = await _townRepository.CreateTown(townDto);
            return newTown;
        }

        public async Task<TownDto> UpdateTown(TownDto townUpdateDto)
        {
            TownDto updatedTown = await _townRepository.UpdateTown(townUpdateDto);

            return updatedTown;
        }

        public async Task DeleteTown(int townId)
        {
            await _townRepository.DeleteTown(townId);
        }

        public async Task<bool> Exists(string title)
        {
            return await _townRepository.Exists(x => x.Title == title);
        }

        public async Task<TownDto> FindTown(string title)
        {
            return await _townRepository.FindTown(title);
        }
    }
}