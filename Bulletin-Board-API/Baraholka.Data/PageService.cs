using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baraholka.Data.Dtos;

namespace Baraholka.Web.Helpers
{
    public class PageService<T> : IPageService<T>
    {
        private IOrderedQueryable<T> QueryableData;
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


        private async Task<List<T>> GetDataForCurrentPage()
        {
            return await QueryableData.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }

        public async Task<PageDataContainer<T>> Paginate(IOrderedQueryable<T> queryAbleData, PageArguments pageParams)
        {
            await InitializePageParams(queryAbleData, pageParams);
            return new PageDataContainer<T>(PageData, PageNumber, PageSize, TotalPages, TotalEntries);
        }

        private async Task InitializePageParams(IOrderedQueryable<T> queryAbleData, PageArguments pageParams)
        {
            QueryableData = queryAbleData;
            TotalEntries = queryAbleData.Count();
            PageSize = pageParams.PageSize;
            TotalPages = (int)Math.Ceiling((double)TotalEntries / PageSize);
            PageNumber = pageParams.PageNumber;
            if (TotalEntries != 0)
            {
                PageData = await GetDataForCurrentPage();
            }
            else
            {
                PageData = new List<T>();
            }
        }

        public static void SetMaxPageSize(int size)
        {
            maxPageSize = size;
        }
    }
}