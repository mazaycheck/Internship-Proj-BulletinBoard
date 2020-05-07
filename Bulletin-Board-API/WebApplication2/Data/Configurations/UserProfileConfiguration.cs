using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(p => p.User).WithOne(t => t.UserProfile).HasForeignKey<UserProfile>(l => l.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
