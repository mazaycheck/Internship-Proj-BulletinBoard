using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class BrandForCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}