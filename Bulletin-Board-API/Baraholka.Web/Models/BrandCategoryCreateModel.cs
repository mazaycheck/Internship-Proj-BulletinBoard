using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class BrandCategoryCreateModel
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string Brand { get; set; }
    }
}