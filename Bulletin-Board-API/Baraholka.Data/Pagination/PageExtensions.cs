using Baraholka.Data.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baraholka.Data.Pagination
{
    public static class PageExtensions
    {
        public static async Task<PageDataContainer<T>> GetPageAsync<T>(this IOrderedQueryable<T> queryAbleData, PageArguments pageParams)
        {
            int maxPageSize = 50;
            int TotalEntries = queryAbleData.Count();
            int PageSize = pageParams.PageSize;

            if (PageSize > maxPageSize) PageSize = maxPageSize;
            else if (PageSize <= 0) PageSize = 10;

            int TotalPages = (int)Math.Ceiling((double)TotalEntries / PageSize);
            int PageNumber = pageParams.PageNumber;

            if (PageNumber <= 0) PageNumber = 1;
            else if (PageNumber > TotalPages) PageNumber = TotalPages;

            List<T> PageData;

            if (TotalEntries != 0)
            {
                PageData = await GetDataForCurrentPage<T>(queryAbleData, PageNumber, PageSize);
            }
            else
            {
                PageData = new List<T>();
            }

            return new PageDataContainer<T>(PageData, PageNumber, PageSize, TotalPages, TotalEntries);
        }

        private static async Task<List<T>> GetDataForCurrentPage<T>(IOrderedQueryable<T> QueryableData, int PageNumber, int PageSize)
        {
            return await QueryableData.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }
    }
}