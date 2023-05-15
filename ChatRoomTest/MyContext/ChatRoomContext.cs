using ChatRoomTest.Models;
using ChatRoomTest.MyContext.Mapping;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomTest.MyContext
{
    public class ChatRoomContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserRelation>  UserRelations { get; set; }

        public ChatRoomContext(DbContextOptions<ChatRoomContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(UserMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);


            base.OnModelCreating(modelBuilder);
        }
    }
}