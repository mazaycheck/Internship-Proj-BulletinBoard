using System.Collections.Generic;

namespace Baraholka.Data.Pagination
{
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