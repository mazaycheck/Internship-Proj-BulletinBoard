using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class TownService : ITownService
    {
        private readonly ITownRepository _townRepository;
        private readonly IMapper _mapper;

        public TownService(ITownRepository townRepository, IMapper mapper)
        {
            _townRepository = townRepository;
            _mapper = mapper;
        }

        public async Task<TownModel> GetTown(int id)
        {
            return _mapper.Map<TownModel>(await _townRepository.GetTown(id));
        }

        public async Task<List<TownModel>> GetAllTowns()
        {
            List<TownDto> towns = await _townRepository.GetAllTowns();

            return _mapper.Map<List<TownDto>, List<TownModel>>(towns);
        }

        public async Task<PageDataContainer<TownModel>> GetPagedTowns(string filter, PageArguments pageArguments)
        {
            filter = filter?.ToLower() ?? string.Empty;
            PageDataContainer<TownDto> pagedTowns = await _townRepository.GetPagedTows(filter, pageArguments);

            if (pagedTowns.PageData.Count == 0)
            {
                return null;
            }

            return _mapper.Map<PageDataContainer<TownModel>>(pagedTowns);
        }

        public async Task<TownModel> CreateTown(TownCreateModel townDto)
        {
            var townToCreate = _mapper.Map<TownDto>(townDto);
            var newTown = await _townRepository.CreateTown(townToCreate);
            return _mapper.Map<TownModel>(newTown);
        }

        public async Task<TownModel> UpdateTown(TownUpdateModel townDto)
        {
            var townToUpdate = _mapper.Map<TownDto>(townDto);

            var updatedTown = await _townRepository.UpdateTown(townToUpdate);

            return _mapper.Map<TownModel>(updatedTown);
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