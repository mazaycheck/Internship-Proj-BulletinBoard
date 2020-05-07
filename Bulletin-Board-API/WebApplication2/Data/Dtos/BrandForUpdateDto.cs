using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
