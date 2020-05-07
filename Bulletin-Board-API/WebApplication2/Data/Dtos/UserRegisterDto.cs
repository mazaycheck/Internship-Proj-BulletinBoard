using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; }  
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MinLength(6)]
        public string Password { get; set; } 
        [Required]
        public int TownId { get; set; }         
        [Required]
        [StringLength(8, ErrorMessage = "The number must be {1} characters long")]
        public string PhoneNumber { get; set; }        
    }
}
