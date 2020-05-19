using System.ComponentModel.DataAnnotations;

namespace Baraholka.Services.Models
{
    public class CategoryCreateModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}