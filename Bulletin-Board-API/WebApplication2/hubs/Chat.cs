using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SignalRChat.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub 
    {

        ClaimsIdentity myidentity;
        
 

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            
        }
        
        public async Task SendPrivateMessage(string user, string message)
        {
            var t = Context.UserIdentifier;
            var y = Context.User.Identity.Name;


            Console.WriteLine(Context.UserIdentifier);
            await Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task UserTypesMessage(string user)
        {
            var t = Context.UserIdentifier;
            var username = Context.User.Identity.Name;
            Console.WriteLine(Context.UserIdentifier);
            await Clients.User(user).SendAsync("OnUserTypesMessage", username);
        }
    }
}