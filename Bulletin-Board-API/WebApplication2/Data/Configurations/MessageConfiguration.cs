using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasOne(m => m.Sender)
                .WithMany(m => m.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(m => m.Reciever)
                .WithMany(m => m.MessagesRecieved)
                .HasForeignKey(m => m.RecieverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
