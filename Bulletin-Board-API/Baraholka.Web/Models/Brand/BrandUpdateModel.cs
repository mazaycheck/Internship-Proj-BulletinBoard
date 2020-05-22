using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
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