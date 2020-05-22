using System.ComponentModel.DataAnnotations;

namespace Baraholka.Services.Models
{
    public class UserRolesUpdateModel
    {
        [Required(ErrorMessage = "Field is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field is required!")]
        public string[] Roles { get; set; }
    }
}