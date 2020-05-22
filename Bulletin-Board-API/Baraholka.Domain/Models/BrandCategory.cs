using System.ComponentModel.DataAnnotations;

namespace Baraholka.Domain.Models
{
    public class BrandCategory
    {
        public int BrandCategoryId { get; set; }

        [Required]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}