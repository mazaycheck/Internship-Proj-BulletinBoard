using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class CategoryUpdateModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}