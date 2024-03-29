﻿
using System;

namespace ChatRoomTest.Models
{
    /// <summary>
    /// This class is using to send a message from user A to User B
    /// </summary>
    public class Message
    {
        public long Id { get; private set; }
        public DateTimeOffset TimeOffset { get; private set; }
        public long FkFromUserId { get; private set; }
        public User FromUser { get; private set; }

        public long FkToUserId { get; private set; }
        public User ToUser { get; private set; }
        
        public string MessageContent { get; private set; }

        public bool Like { get; private set; }

        public bool IsRead { get; private set; }

        /// <summary>
        /// To Create A message from User A To the User B
        /// </summary>
        /// <param name="fkFromUserId">Id of User A that sending a message</param>
        /// <param name="fkToUserId"> Id of user b that get a message</param>
        /// <param name="messageContent">A message exchange between User a to user b</param>
        public Message(long fkFromUserId, long fkToUserId, string messageContent)
        {
            
            FkFromUserId = fkFromUserId;
            FkToUserId = fkToUserId;
            MessageContent = messageContent;
            Like = false;
            IsRead = false;
            TimeOffset = DateTimeOffset.UtcNow;
        }


        public void Edit(string messageContent)
        {
            MessageContent = messageContent;
        }

        public void LikeMessage()
        {
            Like = true;
        }

        public void UnLikeMessage()
        {
            Like = false;
        }

        public void MessageAsRead()
        {
            IsRead = true;
        }
    }
}