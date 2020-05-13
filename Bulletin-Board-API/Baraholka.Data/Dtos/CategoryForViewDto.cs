using System.Collections.Generic;

namespace Baraholka.Data.Dtos
{
    public class CategoryForViewDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Brands { get; set; }
    }
}