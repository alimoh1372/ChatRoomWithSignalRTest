﻿using ChatRoomTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoomTest.MyContext.Mapping
{
    public class UserRelationMapping:IEntityTypeConfiguration<UserRelation>
    {
        public void Configure(EntityTypeBuilder<UserRelation> builder)
        {
            //define table name
            builder.ToTable("UserRelations");
            //Specify the primary key
            builder.HasKey(x => x.Id);

            

            //define property mapping
            


            //Define many-to-many self referencing with User
            builder.HasOne(x => x.UserA)
                .WithMany(x => x.UserARelations)
                .HasForeignKey(x => x.FkUserAId);

            builder.HasOne(x => x.UserB)
                .WithMany(x => x.UserBRelations)
                .HasForeignKey(x => x.FkUserBId);

            //Define Uniq of user a , user b with each other

            builder.HasIndex(x => new {x.FkUserAId, x.FkUserBId})
                .IsUnique();
        }
    }
}