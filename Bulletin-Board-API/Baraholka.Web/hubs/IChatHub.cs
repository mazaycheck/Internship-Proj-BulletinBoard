using System.Threading.Tasks;

namespace Baraholka.Web.hubs
{
    public interface IChatHub
    {
        public Task SendMessage(string user, string message);

        public Task SendPrivateMessage(string user, string message);
    }
}