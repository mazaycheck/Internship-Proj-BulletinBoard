using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
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
