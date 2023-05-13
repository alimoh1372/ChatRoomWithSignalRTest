using System.Threading.Tasks;

namespace ChatRoomTest.Services
{
    public interface IChatRoomService
    {
        Task<bool> SetName(string userName);
    }
}