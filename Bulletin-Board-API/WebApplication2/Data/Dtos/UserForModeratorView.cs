using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Data.Dtos
{
    public class UserForModeratorView
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}
