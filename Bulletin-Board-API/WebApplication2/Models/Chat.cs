using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Chat
    {
        
        public int UserOneId { get; set; }
        public int UserTwoId { get; set; }
        public  User UserOne{get;set;}
        public  User UserTwo{get;set;}
        public ICollection<Message> Messages { get; set; }
    }
}
