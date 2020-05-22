using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;

namespace Baraholka.Data.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int TownId { get; set; }
        public Town Town { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}