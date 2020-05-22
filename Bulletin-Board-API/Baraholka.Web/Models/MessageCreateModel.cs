using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class MessageCreateModel
    {
        [Required]
        [MaxLength(300, ErrorMessage = "Max length {0} characters")]
        public string Text { get; set; }

        [Required]
        public int RecieverId { get; set; }
    }
}