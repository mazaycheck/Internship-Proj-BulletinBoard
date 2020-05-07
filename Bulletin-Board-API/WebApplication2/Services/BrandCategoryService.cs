using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class BrandCategoryService
    {
        private readonly IGenericRepository<BrandCategory> _repo;
        private readonly IMapper _mapper;

        public BrandCategoryService(IGenericRepository<BrandCategory> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<BrandCategoryForViewDto>> GetAll(string category, string brand)
        {
            var dataQuery = _repo.GetAll()
                    .Include(x => x.Category).Include(x => x.Brand)
                    .Where(x => x.Category.Title.Contains(category ?? "") && x.Brand.Title.Contains(brand ?? ""));

            var data = await dataQuery
                .ProjectTo<BrandCategoryForViewDto>(_mapper.ConfigurationProvider).ToListAsync();

            //var data = await dataQuery
            //        .Select(x => new BrandCategoryForViewDto()
            //            {
            //                BrandCategoryId = x.BrandCategoryId,
            //                CategoryId = x.CategoryId,
            //                CategoryTitle = x.Category.Title,
            //                BrandId = x.BrandId,
            //                BrandTitle = x.Brand.Title
            //            })
            //         .ToListAsync();
            return data;
        }
    }
}
