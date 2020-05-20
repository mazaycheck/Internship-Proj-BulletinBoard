
using System.Collections.Generic;

namespace Baraholka.Contracts
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string Title { get; set; }
        public ICollection<BrandCategoryDto> BrandCategories { get; set; }
    }
}
