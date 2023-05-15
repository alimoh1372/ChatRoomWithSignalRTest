using System;
using System.Threading.Tasks;
using ChatRoomTest.Models;
using ChatRoomTest.MyContext;
using ChatRoomTest.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChatRoomTest.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatHub(ChatRoomContext context, IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }


        public async Task SetName(string userName)
        {
            var result =await _chatRoomService.SetName(userName);
            if (!result)
            {
                await Clients.Caller.SendAsync("responseSetName", false);
            }
            else
            {
                //TODO:Implement getting chat history of user and send to its method on client and send all chats with client

                var userListChat =await _chatRoomService.LoadUserNamesChatWithBefore(userName);
                
                var jsonUserList= JsonConvert.SerializeObject(userListChat);
                await Clients.Caller.SendAsync("loadUserNamesChatWithThem", jsonUserList);
            }
        }
    }
}