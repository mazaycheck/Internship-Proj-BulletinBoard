using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Baraholka.Domain.Models
{
    public class User : IdentityUser<int>
    {
        public int? TownId { get; set; }
        public virtual Town Town { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Annoucement> Annoucements { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesRecieved { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}