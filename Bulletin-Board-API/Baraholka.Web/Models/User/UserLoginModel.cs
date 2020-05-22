using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}