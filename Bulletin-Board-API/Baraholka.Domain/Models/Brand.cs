using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Baraholka.Domain.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public ICollection<BrandCategory> BrandCategories { get; set; }
    }
}