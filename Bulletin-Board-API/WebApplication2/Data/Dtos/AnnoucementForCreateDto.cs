using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Data.Dtos
{
    public class AnnoucementForCreateDto
    {        
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum title lengh is 100 characters")]
        public string Title { get; set; }

        [MaxLength(2000, ErrorMessage = "Advert can contain only 2000 characters")]
        public string Description { get; set; }

        [Required]
        [Range(1, 9999999)]
        public int Price { get; set; }
                
        public virtual List<IFormFile> Photo { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int BrandCategoryId { get; set; }        
              
    }
}
