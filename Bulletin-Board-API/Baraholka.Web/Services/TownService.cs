using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Web.Services
{
    public class TownService : ITownService
    {
        private readonly IGenericRepository<Town> _repository;
        private readonly IMapper _mapper;
        private readonly IPageService<Town> _pageService;

        public TownService(IGenericRepository<Town> repository, IMapper mapper, IPageService<Town> pageService)
        {
            _repository = repository;
            _mapper = mapper;
            _pageService = pageService;
        }

        public async Task<List<TownForPublicViewDto>> GetTownsForPublic()
        {
            var references = new string[0];

            var filters = new List<Expression<Func<Town, bool>>>();

            var orderParams = new List<OrderParams<Town>>
            {
                new OrderParams<Town>{ OrderBy = (town) => town.Title, Descending = false}
            };

            List<Town> towns = await _repository.GetAll(references, filters, orderParams);

            return _mapper.Map<List<Town>, List<TownForPublicViewDto>>(towns);
        }

        public async Task<PageDataContainer<TownForAdminViewDto>> GetTownsForAdmin(string filter, PageArguments pageArguments)
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

            IOrderedQueryable<Town> allTowns = _repository.GetAllForPaging(references, filters, orderParams);

            PageDataContainer<Town> pagedTowns = await _pageService.Paginate(allTowns, pageArguments);

            if (pagedTowns.PageData.Count == 0)
            {
                return null;
            }

            return _mapper.Map<PageDataContainer<TownForAdminViewDto>>(pagedTowns);
        }

        public async Task<TownForAdminViewDto> GetTownForAdmin(int id)
        {
            var town = await _repository.GetById(id);
            if (town != null)
            {
                return _mapper.Map<TownForAdminViewDto>(town);
            }
            return null;
        }

        public async Task<TownForAdminViewDto> CreateTown(TownForCreateDto townDto)
        {
            if (await _repository.Exists(x => x.Title == townDto.Title))
            {
                throw new ArgumentException($"Such town already exists : {townDto.Title}");
            }

            Town townToCreate = _mapper.Map<Town>(townDto);
            await _repository.Create(townToCreate);
            return _mapper.Map<TownForAdminViewDto>(townToCreate);
        }

        public async Task<TownForAdminViewDto> UpdateTown(TownForUpdateDto townDto)
        {
            Town townToUpdate = _mapper.Map<Town>(townDto);

            var townFromDb = await _repository.GetSingle(x => x.Title == townDto.Title);

            if (townFromDb != null)
            {
                if (townFromDb.TownId != townDto.TownId)
                {
                    throw new ArgumentException($"Such town already exists: {townDto.Title}");
                }
                _mapper.Map(townToUpdate, townFromDb);
                await _repository.Save();
                return _mapper.Map<TownForAdminViewDto>(townFromDb);
            }
            else
            {
                await _repository.Update(townToUpdate);
            }

            return _mapper.Map<TownForAdminViewDto>(townToUpdate);
        }

        public async Task DeleteTown(int id)
        {
            var town = await _repository.GetById(id);
            if (town == null)
            {
                throw new NullReferenceException($"No such town with id: {id}");
            }

            await _repository.Delete(town);
        }
    }
}