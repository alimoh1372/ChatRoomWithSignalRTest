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
              // await _context.SaveChangesAsync();
            }
            return await Task.FromResult(result);
        }
    }
}