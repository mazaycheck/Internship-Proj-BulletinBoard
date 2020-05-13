using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class CategoryForCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}