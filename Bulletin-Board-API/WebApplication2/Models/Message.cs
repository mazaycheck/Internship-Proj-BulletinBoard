using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int RecieverId { get; set; }
        public User Reciever { get; set; }
        public DateTime? DateTimeSent { get; set; }
        public DateTime? DateTimeRead { get; set; }
        public bool IsRead { get; set; }
    }
}
