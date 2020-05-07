using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;

namespace WebApplication2.Helpers
{
    public class Paged<T>
    {
        private readonly IQueryable<T> QueryAbleData;

        public List<T> PageData;

        private static int maxPageSize = 50;

        private int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set 
            {
                if (value > maxPageSize) _pageSize = maxPageSize;
                else if (value <= 0) _pageSize = 10;
                else _pageSize = value;
            }
        }


        private int _pageNumber;
        public int PageNumber 
        {
            get { return _pageNumber; }
            set 
            {
                if (value <= 0) _pageNumber = 1;
                else if (value > TotalPages) _pageNumber = TotalPages;
                else { _pageNumber = value; }
            }
        }



        public readonly int TotalEntries;

        public int TotalPages { get; set; }


        public Paged(IQueryable<T> queryAbleData, PaginateParams pageParams)
        {
            QueryAbleData = queryAbleData;
            TotalEntries = queryAbleData.Count();
            PageSize = pageParams.PageSize;
            TotalPages = (int)Math.Ceiling((double) TotalEntries / PageSize);
            PageNumber = pageParams.PageNumber;            
        }

        public async Task<List<T>> GetPageData()
        {
            return await QueryAbleData.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }



        public static async Task<Paged<T>> Paginate(IQueryable<T> source, PaginateParams pageParams)
        {
            var paginator = new Paged<T>(source, pageParams);
            paginator.PageData = await paginator.GetPageData();                        
            return paginator;
        }

        public static void SetMaxPageSize(int size)
        {
            maxPageSize = size;
        }
    }
}
