using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.SignalR;

namespace api.Hubs
{
    public class MonitorHub : Hub
    {
        public async Task SendUpdate(MonitorData data)
        {
            await Clients.All.SendAsync("ReceiveUpdate", data);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}