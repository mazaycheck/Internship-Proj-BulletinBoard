using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Dtos
{
    public class CategoryForViewDto
    {
        public int CategoryId { get; set; }                
        public string Title { get; set; }
        public IEnumerable<string> Brands { get; set; }
    }
}
