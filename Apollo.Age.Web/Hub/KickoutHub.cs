namespace Apollo.Age.Web.Hub
{
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web.ViewModels;
    using Microsoft.AspNet.SignalR;

    [Authorize]
    public class KickoutHub : Hub
    {
        public void Send(int adminId)
        {
            var key = adminId.ToString();

            if (CachingService.Get<OnlineUserCaching>(key) == null)
            {
                CachingService.Set<OnlineUserCaching>(key, new OnlineUserCaching
                {
                    AdminId = adminId,
                    AgentId = 0,
                    Context = Context
                });
            }
            else
            {
                var connectionId = CachingService.Get<OnlineUserCaching>(key).Context.ConnectionId;
                Clients.Client(connectionId).KickoutUser(); // Kickout

                CachingService.Set<OnlineUserCaching>(key, new OnlineUserCaching
                {
                    AdminId = adminId,
                    AgentId = 0,
                    Context = Context
                });
            }
        }
    }
}