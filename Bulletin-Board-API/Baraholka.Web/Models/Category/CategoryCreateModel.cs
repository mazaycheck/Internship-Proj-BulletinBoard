using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class CategoryCreateModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}