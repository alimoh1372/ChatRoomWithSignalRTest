using ChatRoomTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoomTest.MyContext.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);



            //Define a self referencing many to many with UserRelation entity
            builder.HasMany(x => x.UserARelations)
                .WithOne(x => x.UserA)
                .HasForeignKey(x => x.FkUserAId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.UserBRelations)
                .WithOne(x => x.UserB)
                .HasForeignKey(x => x.FkUserBId)
                .OnDelete(DeleteBehavior.Restrict);


            //Define a self referencing many to many with message
            builder.HasMany(x => x.FromMessages)
                .WithOne(x => x.FromUser)
                .HasForeignKey(x => x.FkFromUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.ToMessages)
                .WithOne(x => x.ToUser)
                .HasForeignKey(x => x.FkToUserId)
                .OnDelete(DeleteBehavior.NoAction);



            
        }
    }
}