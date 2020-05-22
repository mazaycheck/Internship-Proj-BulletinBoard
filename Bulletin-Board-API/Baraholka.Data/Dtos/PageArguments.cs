using System;
using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class PageArguments
    {
        [Range(0, 1000)]
        public int PageNumber { get; set; }

        [Range(0, 200)]
        public int PageSize { get; set; }
    }
}