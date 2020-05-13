using Baraholka.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baraholka.Data.Configurations
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