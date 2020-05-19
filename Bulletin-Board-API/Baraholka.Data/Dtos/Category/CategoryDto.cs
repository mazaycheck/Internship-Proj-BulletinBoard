using Baraholka.Domain.Models;
using System.Collections.Generic;

namespace Baraholka.Data.Dtos.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public ICollection<BrandCategory> BrandCategories { get; set; }
    }
}