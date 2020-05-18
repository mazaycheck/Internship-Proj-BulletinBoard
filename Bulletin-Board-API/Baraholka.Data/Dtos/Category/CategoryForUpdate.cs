using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class CategoryForUpdateDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}