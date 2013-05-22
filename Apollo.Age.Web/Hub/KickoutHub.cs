namespace Apollo.Age.Web.Hub
{
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web.ViewModels;
    using Microsoft.AspNet.SignalR;
    using System.Web.Security;
        

    [Authorize]
    public class KickoutHub : Hub
    {
       

        public void Send(int adminId, bool isInternal)
        {
            if (isInternal)
            {
                var key = adminId.ToString();
                var connectionId = string.Empty;

                if (CachingService.Get<OnlineUser>(key) != null)
                {
                    connectionId = CachingService.Get<OnlineUser>(key).ConnectionId;

                    if (!Apollo.Age.Configurations.AppSettings.MultipleSignOn)
                    {
                        lock (connectionId)
                        {
                            Clients.Client(connectionId).kickoutUser(); // Kickout               
                        }
                    }

                }

                CachingService.Set<OnlineUser>(key, new OnlineUser
                {
                    AdminId = adminId,                 
                    ConnectionId = Context.ConnectionId
                });
            }           
        }
       
    }
}