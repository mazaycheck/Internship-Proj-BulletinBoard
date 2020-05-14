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

namespace Baraholka.Services
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

        public async Task<PageDataContainer<UserForModeratorView>> GetUsers(PageArguments pageArguments, string query)
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
            var conditions = new List<Expression<Func<User, bool>>>
            {
                u => u.Id == id
            };
            var user = await _repository.GetSingle(includes, conditions);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserForPublicDetail>(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _repository.FindById(id);
            if (user == null)
            {
                throw new NullReferenceException("No such user");
            }
            await _repository.Delete(user);
        }
    }
}