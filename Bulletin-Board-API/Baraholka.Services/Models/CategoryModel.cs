using System.Collections.Generic;

namespace Baraholka.Services.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Brands { get; set; }
    }
}