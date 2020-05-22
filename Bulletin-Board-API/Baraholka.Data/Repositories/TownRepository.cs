using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Pagination;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Data.Repositories
{
    public class TownRepository : GenericRepository<Town>, ITownRepository
    {
        private readonly IMapper _mapper;

        public TownRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<TownDto> GetTown(int id)
        {
            var townFromDb = await GetSingle(t => t.TownId == id);
            return _mapper.Map<TownDto>(townFromDb);
        }

        public async Task<TownDto> FindTown(string title)
        {
            title = title.ToLower();
            var townFromDb = await GetSingle(t => t.Title == title);
            return _mapper.Map<TownDto>(townFromDb);
        }

        public async Task<bool> TownExists(string title)
        {
            title = title.ToLower();
            return await Exists(t => t.Title.ToLower().Equals(title));
        }

        public async Task<List<TownDto>> GetAllTowns()
        {
            var references = new string[0];

            var filters = new List<Expression<Func<Town, bool>>>();

            var orderParams = new List<OrderParams<Town>>
            {
                new OrderParams<Town>{ OrderBy = (town) => town.Title, Descending = false}
            };

            var towns = await GetAll(references, filters, orderParams);

            return _mapper.Map<List<Town>, List<TownDto>>(towns);
        }

        public async Task<PageDataContainer<TownDto>> GetPagedTows(string filter, PageArguments pageArguments)
        {
            var references = new string[0];

            var filters = new List<Expression<Func<Town, bool>>>()
            {
                town => town.Title == filter
            };

            var orderParams = new List<OrderParams<Town>>
            {
                new OrderParams<Town>{ OrderBy = (town) => town.Title, Descending = false}
            };

            PageDataContainer<Town> pagedTowns = await GetPagedData(references, filters, orderParams, pageArguments);

            return _mapper.Map<PageDataContainer<TownDto>>(pagedTowns);
        }

        public async Task<TownDto> CreateTown(TownDto townDto)
        {
            var townToCreate = _mapper.Map<Town>(townDto);
            var newTown = await CreateAndReturn(townToCreate);
            return _mapper.Map<TownDto>(newTown);
        }

        public async Task<TownDto> UpdateTown(TownDto townDto)
        {
            var townToUpdate = _mapper.Map<Town>(townDto);
            var updatedTown = await UpdateAndReturn(townToUpdate);
            return _mapper.Map<TownDto>(updatedTown);
        }

        public async Task DeleteTown(int townId)
        {
            await Delete(new Town { TownId = townId });
        }
    }
}