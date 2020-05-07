using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationDate : ControllerBase
    {

        [HttpPost]    
        public JsonResult ValidateDateEqualOrGreater([FromBody]DateTime Date)
        {
         
            if (Date >= DateTime.Now)
            {
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }
    }
}
