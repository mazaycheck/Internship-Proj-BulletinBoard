using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class UserRegisterModel
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
        [StringLength(12, ErrorMessage = "The {0} number must be {2} characters long", MinimumLength = 6)]
        public string PhoneNumber { get; set; }
    }
}