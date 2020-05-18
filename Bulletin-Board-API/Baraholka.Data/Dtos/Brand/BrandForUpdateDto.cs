using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class BrandForUpdateDto
    {
        [Required]
        public int BrandId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string[] Categories { get; set; }
    }
}