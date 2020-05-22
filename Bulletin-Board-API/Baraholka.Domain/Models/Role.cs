using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Baraholka.Domain.Models
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}