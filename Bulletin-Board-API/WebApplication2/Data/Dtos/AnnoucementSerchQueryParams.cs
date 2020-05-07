using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Data.Dtos
{
    public class AnnoucementFilter
    {
        public string Category { get; set; }

        public string Query { get; set; }

        public int UserId { get; set; }
    }
}
