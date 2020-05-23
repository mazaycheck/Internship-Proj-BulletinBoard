using System.Collections.Generic;

namespace Baraholka.Web.Models
{
    public class CategoryWebModel
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Brands { get; set; }
    }
}