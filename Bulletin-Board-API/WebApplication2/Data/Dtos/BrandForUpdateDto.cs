using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data.Dtos
{
    public class BrandForUpdateDto
    {
        [Required]
        public int BrandId { get; set; }

        public string Title { get; set; }
        public string[] Categories { get; set; }
    }
}