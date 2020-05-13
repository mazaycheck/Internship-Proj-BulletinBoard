using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CategoryForViewDto>> GetAllCategories(string filter)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryForViewDto, Category>().ReverseMap();
            });
            var includes = new string[]
            {
                $"{nameof(Category.BrandCategories)}.{nameof(Brand)}",
            };

            var orderParams = new List<OrderParams<Category>>
            {
                new OrderParams<Category> { OrderBy = (x) => x.Title, Descending = false }
            };

            var filters = new List<Expression<Func<Category, bool>>>
            {
                category => category.Title.Contains(filter ?? "")
            };

            List<Category> categories = await _repository.GetAll(includes, filters, orderParams);

            if (categories.Count == 0)
            {
                return null;
            }

            return _mapper.Map<List<Category>, List<CategoryForViewDto>>(categories);
        }

        public async Task<CategoryForViewDto> GetCategory(int id)
        {
            var includes = new string[]
            {
                $"{nameof(Category.BrandCategories)}.{nameof(Brand)}",
            };

            Expression<Func<Category, bool>> condition = x => x.CategoryId == id;

            Category category = await _repository.GetSingle(condition, includes);

            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryForViewDto>(category);
        }

        public async Task<CategoryForViewDto> CreateCategory(CategoryForCreateDto newCategory)
        {
            if (await _repository.Exists(x => x.Title == newCategory.Title))
            {
                throw new ArgumentException("Such category already exists");
            }
            Category category = _mapper.Map<Category>(newCategory);
            await _repository.Create(category);
            return _mapper.Map<CategoryForViewDto>(category);
        }

        public async Task<CategoryForViewDto> UpdateCategory(CategoryForCreateDto newCategory)
        {
            if (await _repository.Exists(x => x.Title == newCategory.Title))
            {
                throw new ArgumentException("Such category already exists");
            }
            Category category = _mapper.Map<Category>(newCategory);
            await _repository.Update(category);
            return _mapper.Map<CategoryForViewDto>(category);
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _repository.GetById(id);
            if (category != null)
            {
                await _repository.Delete(category);
            }
            else
            {
                throw new NullReferenceException($"No such category with id: {id}");
            }
        }
    }
}