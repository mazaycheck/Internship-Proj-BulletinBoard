using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}