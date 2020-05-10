using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Helpers;

namespace WebApplication2.Services
{
    public interface IBrandService
    {
        Task<PageService<BrandForViewDto>> GetAllBrands(AnnoucementFilter filterOptions,
            PaginateParams paginateParams, OrderParams orderParams);
    }
}
