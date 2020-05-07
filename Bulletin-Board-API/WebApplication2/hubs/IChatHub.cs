using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.hubs
{
    public interface IChatHub
    {
        public Task SendMessage(string user, string message);


        public Task SendPrivateMessage(string user, string message);
   
    }
}
