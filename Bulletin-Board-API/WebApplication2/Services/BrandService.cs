using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepo;
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IGenericRepository<BrandCategory> _brandCategoryRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public BrandService(IGenericRepository<Brand> brandRepo, IGenericRepository<Category> categoryRepo, IGenericRepository<BrandCategory> brandCategoryRepo, IMapper mapper, AppDbContext context)
        {
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
            _brandCategoryRepo = brandCategoryRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<PageService<BrandForViewDto>> GetAllBrands(AnnoucementFilter filterOptions,
            PaginateParams paginateParams, OrderParams orderParams)
        {
            //var brands = _brandRepo.GetAllQueryable().OrderBy(x => x.Title).Include(x => x.BrandCategories).ThenInclude(x => x.Category)
            // .Select(x => new BrandForViewDto
            // {
            //     BrandId = x.BrandId,
            //     Title = x.Title,
            //     Categories = x.BrandCategories.Select(x => x.Category.Title)
            // });
            

            var filters = new List<Expression<Func<Brand, bool>>>()
            {
                brand => brand.Title.Contains(filterOptions.Query ?? ""),                
            };

            var includes = new string[] { "BrandCategories.Category" };


            var orderParameters = new List<Expression<Func<Brand, object>>>()
            {
                x => x.Title
            };            

            List<Brand> brands = await _brandRepo.GetAll(includes, filters, orderParameters);

            if(brands.Count > 0)
            {

            }
            
            //var filtered = brands.Where(x => x.Title.Contains(filterOptions.Query ?? ""));
            //if (filtered.Count() > 0)
            //{
            //    var paginatedData = await PagedDataContainer<BrandForViewDto>.Paginate(filtered, paginateParams);
            //    return paginatedData;
            //}
            return null;
        }



    }
}
