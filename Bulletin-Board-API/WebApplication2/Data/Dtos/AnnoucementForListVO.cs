using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Data.Dtos
{
    public class AnnoucementsForListVO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Town { get; set; }
        public DateTime? Date { get; set; }
        public string Category { get; set; }
        public int UserId { get; set; }
        public virtual List<string> PhotoUrls { get; set; }
    }
}
