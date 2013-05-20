namespace Apollo.Age.Web.Hub
{
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web.ViewModels;
    using Microsoft.AspNet.SignalR;

    public class KickoutHub : Hub
    {
        public void Send()
        {
            var key = SessionManager.CurrentSession.AdminId + "_" + SessionManager.CurrentSession.AgentId;
            
            if (CachingService.Get<OnlineUserCaching>(key) == null)
            {
                CachingService.Set<OnlineUserCaching>(key, new OnlineUserCaching
                {
                    AdminId = SessionManager.CurrentSession.AdminId,
                    AgentId = SessionManager.CurrentSession.AgentId,
                    Context = Context
                });
                                
            }
            else
            {
                var connectionId = CachingService.Get<OnlineUserCaching>(key).Context.ConnectionId;
                Clients.Client(connectionId).KickoutUser(); // Kickout

               CachingService.Set<OnlineUserCaching>(key, new OnlineUserCaching
               {
                   AdminId = SessionManager.CurrentSession.AdminId,
                   AgentId = SessionManager.CurrentSession.AgentId,
                   Context = Context
               });
            }
        }
    }
}