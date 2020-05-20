using System.Collections.Generic;

namespace Baraholka.Services.Models
{
    public class BrandModel
    {
        public int BrandId { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}