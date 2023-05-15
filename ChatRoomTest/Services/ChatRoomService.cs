using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ChatRoomTest.Models;
using ChatRoomTest.MyContext;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomTest.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly ChatRoomContext _context;

        public ChatRoomService(ChatRoomContext context)
        {
            _context = context;
        }

        public async Task<bool> SetName(string userName)
        {
            bool result = await _context.Users.AnyAsync(x => x.Name.ToLower() == userName.ToLower());
            if (!result)
            {
                User user = new User(userName);
                _context.Add(user);
                await _context.SaveChangesAsync();
            }
            return await Task.FromResult(result);
        }

        public async Task<List<UserViewModel>> LoadUserNamesChatWithBefore(string userName)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            //var userList = _context.Users.Include(x=>x.ToMessages)
            //    .Include(x=>x.FromMessages)
            //    .Where(u => u.Name == userName)
            //    .Where(x=>x.UserARelations).ToList();
            var userId = _context.Users.FirstOrDefault(x => x.Name == userName)?.Id;
            if (userId == null)
                return users;
            var userList = await _context.Messages.Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .Where(m => m.FkFromUserId == userId || m.FkToUserId == userId)
                .Select(m => new UserViewModel
                {
                    Id = m.FkFromUserId == userId ? m.FkToUserId : m.FkFromUserId,
                    UserName = m.FkFromUserId == userId ? m.ToUser.Name : m.FromUser.Name
                }).Distinct().ToListAsync();


            return await Task.FromResult(userList);

        }

        public List<User> SearchUsers(string userName, long currentUser)
        {
            throw new System.NotImplementedException();
        }

        public async Task<long> GetIdBy(string userName)
        {
            var id = _context.Users.FirstOrDefault(x => x.Name == userName).Id;
            return await Task.FromResult(id);
        }

        public async Task<List<MessageViewModel>> GetChatHistory(long currentUserId, long receiverUserId)
        {
            var query = _context.Messages.Where(x =>
                x.FkFromUserId == currentUserId || x.FkFromUserId == receiverUserId || x.FkToUserId == currentUserId ||
                x.FkToUserId == receiverUserId).
                Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .Select(x => new MessageViewModel
                {
                    Id = x.Id,
                    FkSenderUserId = x.FkFromUserId,
                    FkSenderUserName = x.FromUser.Name,
                    FkReceiverUserid = x.FkToUserId,
                    FkReceiverUserName = x.ToUser.Name,
                    MessageContent = x.MessageContent,
                    CreationDate = x.TimeOffset
                }).OrderBy(x => x.Id).AsNoTracking();
            return await query.Distinct().ToListAsync();
        }

        public async Task<bool> AddMessage(long currentUserId, long activeUserId, string messageContent)
        {
            Message message = new Message(currentUserId, activeUserId, messageContent);
            _context.Messages.Add(message);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<MessageViewModel> GetLatestMessageBy(long senderId, long receiverId)
        {
            return await _context.Messages
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .Select(x => new MessageViewModel
                {
                    Id = x.Id,
                    FkSenderUserId = x.FkFromUserId,
                    FkSenderUserName = x.FromUser.Name,
                    FkReceiverUserid = x.FkToUserId,
                    FkReceiverUserName = x.ToUser.Name,
                    MessageContent = x.MessageContent,
                    CreationDate = x.TimeOffset
                })
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => (x.FkSenderUserId == senderId && x.FkReceiverUserid == receiverId) ||
                                    (x.FkSenderUserId == receiverId && x.FkReceiverUserid == senderId));

        }
    }
}