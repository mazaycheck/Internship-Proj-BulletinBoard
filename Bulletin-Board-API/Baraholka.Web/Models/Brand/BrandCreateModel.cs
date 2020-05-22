using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class BrandCreateModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }
    }
}