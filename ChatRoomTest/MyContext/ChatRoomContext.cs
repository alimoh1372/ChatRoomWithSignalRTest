using ChatRoomTest.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomTest.MyContext
{
    public class ChatRoomContext:DbContext
    {
        public DbSet<User> Users { get; set; }
    

        public ChatRoomContext(DbContextOptions<ChatRoomContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}