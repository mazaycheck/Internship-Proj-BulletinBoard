using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Data.Dtos
{
    public class MessageForCreateDto
    {
        [Required]
        [MaxLength(300, ErrorMessage = "Max length {0} characters")]
        public string Text { get; set; }

        public int SenderId { get; set; }

        [Required]
        public int RecieverId { get; set; }
    }
}