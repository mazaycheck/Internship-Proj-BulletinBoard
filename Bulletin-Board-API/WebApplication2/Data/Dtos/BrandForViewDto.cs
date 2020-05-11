using System;
using System.Collections.Generic;

namespace WebApplication2.Data.Dtos
{
    public class BrandForViewDto
    {
        public int BrandId { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Categories { get; set; }

    }
}