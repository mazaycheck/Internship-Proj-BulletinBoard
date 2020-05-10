using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;

namespace WebApplication2.Helpers
{
    public class PageService<T> : IPageService<T>
    {
        private IQueryable<T> QueryableData;
        private readonly IMapper _mapper;
        private static int maxPageSize = 50;
        private int _pageSize;
        private int _pageNumber;
        private List<T> PageData;
        private int TotalEntries { get; set; }
        private int TotalPages { get; set; }

        private int PageSize

        {
            get { return _pageSize; }
            set
            {
                if (value > maxPageSize) _pageSize = maxPageSize;
                else if (value <= 0) _pageSize = 10;
                else _pageSize = value;
            }
        }

        private int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if (value <= 0) _pageNumber = 1;
                else if (value > TotalPages) _pageNumber = TotalPages;
                else { _pageNumber = value; }
            }
        }

        public PageService(IMapper mapper)
        {
            _mapper = mapper;
        }

        private async Task<List<T>> GetDataForCurrentPage()
        {
            return await QueryableData.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }

        public async Task<PageDataContainer<T>> Paginate(IQueryable<T> queryAbleData, PaginateParams pageParams)
        {
            await InitializePageParams(queryAbleData, pageParams);
            return new PageDataContainer<T>(PageData, PageNumber, PageSize, TotalPages, TotalEntries);
        }

        private async Task InitializePageParams(IQueryable<T> queryAbleData, PaginateParams pageParams)
        {
            QueryableData = queryAbleData;
            TotalEntries = queryAbleData.Count();
            PageSize = pageParams.PageSize;
            TotalPages = (int)Math.Ceiling((double)TotalEntries / PageSize);
            PageNumber = pageParams.PageNumber;
            PageData = await GetDataForCurrentPage();
        }

        public static void SetMaxPageSize(int size)
        {
            maxPageSize = size;
        }

        public PageDataContainer<U> TransformData<U>()
        {
            var data = PageData.Select(x => _mapper.Map<U>(x)).ToList();

            return new PageDataContainer<U>(data, PageNumber, PageSize, TotalPages, TotalEntries);
        }
    }

    public class PageDataContainer<U>
    {
        public readonly List<U> PageData;
        public readonly int PageNumber;
        public readonly int PageSize;
        public readonly int TotalEntries;
        public readonly int TotalPages;

        public PageDataContainer(List<U> pageData, int pageNumber, int pageSize, int totalPages, int totalEntries)
        {
            PageData = pageData;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalEntries = totalEntries;
        }
    }
}