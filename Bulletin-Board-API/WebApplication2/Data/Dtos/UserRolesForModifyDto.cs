using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Data.Dtos
{
    public class UserRolesForModifyDto
    {
        public string Email { get; set; }
        public string[] NewRoles { get; set; }
    }
}
