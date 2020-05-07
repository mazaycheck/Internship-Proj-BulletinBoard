using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Data.Dtos
{
    public class PaginateParams
    {

        [Range(0,1000)]        
        public int PageNumber { get; set; }


        [Range(0, 200)]        
        public int PageSize { get; set; }
    }
}
