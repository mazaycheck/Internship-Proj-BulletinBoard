using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            
            //builder.HasOne(x => x.UserOne)
            //    .WithMany(x => x.Chats)
            //    .HasForeignKey(x => x.UserOneId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(x => x.UserTwo)
            //    .WithMany()
            //    .HasForeignKey(x => x.UserTwoId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasKey(x => new { x.UserOneId, x.UserTwoId });


        }
    }
}
