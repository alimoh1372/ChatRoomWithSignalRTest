using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatRoomTest.Hubs
{
    public class ChatHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceiveMessage",
                Context.ConnectionId,
                DateTimeOffset.UtcNow,
                "Hello Dear Welcome to the chatRoom");

            await base.OnConnectedAsync();
        }
    }
}