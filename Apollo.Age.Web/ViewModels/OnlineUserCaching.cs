using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
namespace Apollo.Age.Web.ViewModels
{
    public class OnlineUser
    {
        public int AdminId { get; set; }                

        public string ConnectionId { get; set; }
    }
}