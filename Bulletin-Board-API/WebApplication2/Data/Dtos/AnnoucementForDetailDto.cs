using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Dtos
{
    public class AnnoucementForViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public virtual List<string> PhotoUrls { get; set; }
        public int UserId { get; set; }        
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Town { get; set; }

    }
}
