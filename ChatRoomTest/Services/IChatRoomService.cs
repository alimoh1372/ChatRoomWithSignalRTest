using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatRoomTest.Models;

namespace ChatRoomTest.Services
{
    public interface IChatRoomService
    {
        Task<bool> SetName(string userName);
        //Load user names that current user chat with them
        Task<List<UserViewModel>> LoadUserNamesChatWithBefore(string userName);
        List<User> SearchUsers(string userName, long CurrentUser);
        Task<long> GetIdBy(string userName);

        Task<List<MessageViewModel>> GetChatHistory(long currentUserId, long receiverUserId);
        Task<bool> AddMessage(long currentUserId, long activeUserId, string messageContent);
        MessageViewModel GetLatestMessageBy(long senderId, long receiverId);
    }

    public class MessageViewModel
    {
        public long Id { get; set; }
        public long FkSenderUserId { get; set; }
        public string FkSenderUserName { get; set; }
        public long FkReceiverUserid { get; set; }
        public string FkReceiverUserName { get; set; }
        public string MessageContent { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}