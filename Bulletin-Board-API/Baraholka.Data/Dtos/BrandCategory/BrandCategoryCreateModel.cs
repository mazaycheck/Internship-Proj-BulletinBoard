using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class BrandCategoryCreateModel
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string Brand { get; set; }
    }
}