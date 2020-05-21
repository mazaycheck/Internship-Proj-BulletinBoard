using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class UserRolesUpdateModel
    {
        [Required(ErrorMessage = "Field is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field is required!")]
        public string[] Roles { get; set; }
    }
}