namespace Apollo.Age.Web.Hub
{
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web.ViewModels;
    using Microsoft.AspNet.SignalR;
    using System.Web.Security;

    
    public class KickoutHub : Hub
    {
        public void Send( int adminId, int agentId )
        {
            var key = adminId.ToString() + "_" + agentId.ToString();
            var connectionId = string.Empty;        
            if (CachingService.Get<OnlineUserCaching>(key) != null)
            {
                connectionId = CachingService.Get<OnlineUserCaching>(key).ConnectionId;

                if (!Apollo.Age.Configurations.AppSettings.MultipleSignOn)
                {
                    Clients.Client(connectionId).kickoutUser(); // Kickout               
                }
                
            }
           
            CachingService.Set<OnlineUserCaching>(key, new OnlineUserCaching
            {
                AdminId = adminId,
                AgentId = agentId,
                ConnectionId = Context.ConnectionId
            });
                        
        }
    }
}