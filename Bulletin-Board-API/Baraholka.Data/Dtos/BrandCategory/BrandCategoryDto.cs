using Baraholka.Domain.Models;

namespace Baraholka.Data.Dtos
{
    public class BrandCategoryDto
    {
        public int BrandCategoryId { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}