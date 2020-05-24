using System;

namespace Baraholka.Web.Models
{
    public class UserAuditWebModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public bool IsActive { get; set; }
    }
}