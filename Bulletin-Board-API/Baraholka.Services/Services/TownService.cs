using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class TownService : ITownService
    {
        private readonly IGenericRepository<Town> _townRepository;
        private readonly IMapper _mapper;

        public TownService(IGenericRepository<Town> repository, IMapper mapper)
        {
            _townRepository = repository;
            _mapper = mapper;
        }

        public async Task<List<TownForPublicViewDto>> GetTownsForPublic()
        {
            var references = new string[0];

            var filters = new List<Expression<Func<Town, bool>>>();

            var orderParams = new List<OrderParams<Town>>
            {
                new OrderParams<Town>{ OrderBy = (town) => town.Title, Descending = false}
            };

            List<Town> towns = await _townRepository.GetAll(references, filters, orderParams);

            return _mapper.Map<List<Town>, List<TownForPublicViewDto>>(towns);
        }

        public async Task<PageDataContainer<TownServiceDto>> GetTownsForAdmin(string filter, PageArguments pageArguments)
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

            PageDataContainer<Town> pagedTowns = await _townRepository.GetPagedData(references, filters, orderParams, pageArguments);

            if (pagedTowns.PageData.Count == 0)
            {
                return null;
            }

            return _mapper.Map<PageDataContainer<TownServiceDto>>(pagedTowns);
        }

        public async Task<TownServiceDto> CreateTown(TownForCreateDto townDto)
        {
            Town townToCreate = _mapper.Map<Town>(townDto);
            await _townRepository.Create(townToCreate);
            return _mapper.Map<TownServiceDto>(townToCreate);
        }

        public async Task<TownServiceDto> UpdateTown(TownForUpdateDto townDto)
        {
            Town townToUpdate = _mapper.Map<Town>(townDto);

            var townFromDb = await _townRepository.GetFirst(x => x.Title == townDto.Title);

            if (townFromDb != null)
            {
                if (townFromDb.TownId != townDto.TownId)
                {
                    throw new ArgumentException($"Such town already exists: {townDto.Title}");
                }
                _mapper.Map(townToUpdate, townFromDb);
                await _townRepository.Save();
                return _mapper.Map<TownServiceDto>(townFromDb);
            }
            else
            {
                await _townRepository.Update(townToUpdate);
            }

            return _mapper.Map<TownServiceDto>(townToUpdate);
        }

        public async Task DeleteTown(TownServiceDto townDto)
        {
            var townToDelete = _mapper.Map<Town>(townDto);
            await _townRepository.Delete(townToDelete);
        }

        public async Task<bool> Exists(string title)
        {
            return await _townRepository.Exists(x => x.Title == title);
        }

        public async Task<TownServiceDto> FindTown(int id)
        {
            var town = await _townRepository.GetSingle(x => x.TownId == id);
            if (town != null)
            {
                return _mapper.Map<TownServiceDto>(town);
            }
            return null;
        }
    }
}