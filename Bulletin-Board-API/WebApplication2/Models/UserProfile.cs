using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class UserProfile
    {
        [ForeignKey("User")]
        public int Id { get; set; }
        public User User { get; set; }
    }
}
