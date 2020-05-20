
using System.Collections.Generic;

namespace Baraholka.Contracts
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public ICollection<BrandCategoryDto> BrandCategories { get; set; }
    }
}