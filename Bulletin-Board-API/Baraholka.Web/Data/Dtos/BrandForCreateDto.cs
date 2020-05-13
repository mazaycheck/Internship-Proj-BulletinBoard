using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Data.Dtos
{
    public class BrandForCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}