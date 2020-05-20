using System.ComponentModel.DataAnnotations;

namespace Baraholka.Services.Models
{
    public class BrandUpdateModel
    {
        [Required]
        public int BrandId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }

        public string[] Categories { get; set; }
    }
}