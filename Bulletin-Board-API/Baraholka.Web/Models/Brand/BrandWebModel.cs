using System.Collections.Generic;

namespace Baraholka.Web.Models
{
    public class BrandWebModel
    {
        public int BrandId { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}