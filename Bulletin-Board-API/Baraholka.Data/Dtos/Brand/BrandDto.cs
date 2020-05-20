using Baraholka.Domain.Models;
using System.Collections.Generic;

namespace Baraholka.Data.Dtos
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string Title { get; set; }
        public ICollection<BrandCategory> BrandCategories { get; set; }
    }
}