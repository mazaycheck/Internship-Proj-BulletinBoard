using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data.Dtos
{
    public class UserRolesForModifyDto
    {
        [Required(ErrorMessage = "Field is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field is required!")]
        public string[] Roles { get; set; }
    }
}