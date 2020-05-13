using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Data.Dtos
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