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
            var result = await _chatRoomService.SetName(userName);

            if (!result)
            {
                await Clients.Caller.SendAsync("responseSetName", false);
            }
            else
            {
                //TODO:Implement getting chat history of user and send to its method on client and send all chats with client
                var userListChat = await _chatRoomService.LoadUserNamesChatWithBefore(userName);

                var jsonUserList = JsonConvert.SerializeObject(userListChat);
                long currentUserId = await _chatRoomService.GetIdBy(userName);
                await Clients.Caller.SendAsync("loadUserNamesChatWithThem", jsonUserList, currentUserId);
            }
        }

        public async Task LoadChatHistory(long currentUserId, long activeUserId)
        {
            var chatHistory = await _chatRoomService.GetChatHistory(currentUserId, activeUserId);
            var jsonList = JsonConvert.SerializeObject(chatHistory);
            await Clients.Caller.SendAsync("responseOfLoadChatHistory", chatHistory);
        }

        public async Task SendMessage(long currentUserId, long activeUserId, string messageContent)
        {
            var result = await _chatRoomService.AddMessage(currentUserId, activeUserId, messageContent);
            if (!result)
                return;
            try
            {
                var latestMessage =await _chatRoomService.GetLatestMessageBy(currentUserId, activeUserId);
                
                await Clients.All.SendAsync("ReceiveMessage", latestMessage.Id, latestMessage.FkSenderUserId,
                    latestMessage.FkSenderUserName
                    , latestMessage.FkReceiverUserName, latestMessage.FkReceiverUserName, latestMessage.CreationDate,
                    latestMessage.MessageContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
           
        }
    }
}