using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ChatRoomTest.Models;
using ChatRoomTest.MyContext;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomTest.Services
{
    public class ChatRoomService:IChatRoomService
    {
        private readonly ChatRoomContext _context;

        public ChatRoomService(ChatRoomContext context)
        {
            _context = context;
        }

        public  async Task<bool> SetName(string userName)
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
            var userList =await _context.Messages.Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .Where(m => m.FkFromUserId == userId || m.FkToUserId == userId)
                .Select(m=>new UserViewModel
                {
                 Id   = m.FkFromUserId==userId?m.FkToUserId:m.FkFromUserId,
                 UserName = m.FkFromUserId==userId?m.ToUser.Name:m.FromUser.Name
                }).ToListAsync();

              
            return await Task.FromResult(userList);

        }

        public List<User> LoadAllUser()
        {
            throw new System.NotImplementedException();
        }
    }
}