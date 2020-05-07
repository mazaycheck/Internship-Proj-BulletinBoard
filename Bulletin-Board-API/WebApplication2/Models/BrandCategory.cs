using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class BrandCategory
    {
        
        public int BrandCategoryId { get; set; }
        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public override string ToString()
        {
            return $"BrandId: {BrandId}, CategoryId: {CategoryId}";
        }
    }
}
