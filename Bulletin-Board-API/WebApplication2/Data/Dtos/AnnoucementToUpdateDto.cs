using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Dtos
{
    public class AnnoucementPartialUpdateDto
    {
        [Required(ErrorMessage = "Id is required!")]
        public int AnnoucementId { get; set; }

        [MaxLength(100, ErrorMessage = "Maximum title lengh is 100 characters")]
        public string Title { get; set; }

        [MaxLength(2000, ErrorMessage = "Advert can contain only 2000 characters")]
        public string Description { get; set; }

        [Range(1, 9999999, ErrorMessage = "Price must be in range from {0} to {1}")]
        public int Price { get; set; }    
        
        public virtual List<Photo> Photos { get; set; }

        public bool IsActive { get; set; }
    }
}
