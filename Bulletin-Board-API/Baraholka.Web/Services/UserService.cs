using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Baraholka.Web.Data.Dtos;
using Baraholka.Web.Data.Repositories;
using Baraholka.Web.Helpers;
using Baraholka.Domain.Models;

namespace Baraholka.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPageService<User> _pageService;

        public UserService(IUserRepository repository, IMapper mapper, IPageService<User> pageService)
        {
            _repository = repository;
            _mapper = mapper;
            _pageService = pageService;
        }

        public async Task<PageDataContainer<UserForModeratorView>> GetUsers([FromQuery] PageArguments pageArguments, [FromQuery] string query)
        {
            IOrderedQueryable<User> users = _repository.PrepareUsersForPaging(query);

            PageDataContainer<User> pagedUseres = await _pageService.Paginate(users, pageArguments);
            if (pagedUseres.PageData.Count > 0)
            {
                PageDataContainer<UserForModeratorView> pagedUserDtos = _mapper.Map<PageDataContainer<UserForModeratorView>>(pagedUseres);
                return pagedUserDtos;
            }

            return null;
        }

        public async Task<UserForPublicDetail> GetUser(int id)
        {
            var includes = new string[] { $"{nameof(Town)}" };
            var user = await _repository.GetSingle(u => u.Id == id, includes);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserForPublicDetail>(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null)
            {
                throw new NullReferenceException("No such user");
            }
            await _repository.Delete(user);
        }
    }
}