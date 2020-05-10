using System;

namespace WebApplication2.Data.Dtos
{
    public class UserForPublicDetail
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}