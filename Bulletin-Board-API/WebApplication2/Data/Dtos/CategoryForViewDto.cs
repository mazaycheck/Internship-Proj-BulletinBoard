using System.Collections.Generic;

namespace WebApplication2.Data.Dtos
{
    public class CategoryForViewDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Brands { get; set; }
    }
}