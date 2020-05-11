using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data.Dtos
{
    public class BrandForCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
