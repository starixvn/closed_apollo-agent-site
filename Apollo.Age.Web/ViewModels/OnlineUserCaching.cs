using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
namespace Apollo.Age.Web.ViewModels
{
    public class OnlineUserCaching
    {
        public int AdminId { get; set; }

        public int AgentId { get; set; }

        public string ConnectionId { get; set; }
    }
}