using System.ComponentModel.DataAnnotations;

namespace Baraholka.Services.Models
{
    public class BrandCreateModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }
    }
}