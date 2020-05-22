using System.ComponentModel.DataAnnotations;

namespace Baraholka.Services.Models
{
    public class BrandCategoryCreateModel
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string Brand { get; set; }
    }
}