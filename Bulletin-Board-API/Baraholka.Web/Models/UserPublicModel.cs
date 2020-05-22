using System;

namespace Baraholka.Web.Models
{
    public class UserPublicWebModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}