using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class User : IdentityUser<int>
    {

        public UserProfile UserProfile { get; set; }        
        public int? TownId { get; set; }
        public virtual Town Town { get; set; }

        [Range(typeof(DateTime), "01/01/2020", "01/01/2100")]
        public DateTime RegistrationDate { get; set; }
        public virtual ICollection<Annoucement> Annoucements { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesRecieved { get; set; }        
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
