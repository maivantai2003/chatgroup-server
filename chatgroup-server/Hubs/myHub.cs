using Microsoft.AspNetCore.SignalR;

namespace chatgroup_server.Hubs
{
    public class myHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
