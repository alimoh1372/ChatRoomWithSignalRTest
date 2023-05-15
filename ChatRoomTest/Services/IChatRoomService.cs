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
        List<User> LoadAllUser();
    }
}