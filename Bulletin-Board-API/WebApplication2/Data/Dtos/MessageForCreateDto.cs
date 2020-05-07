using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Data.Dtos
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
