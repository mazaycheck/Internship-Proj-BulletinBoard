using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId });

            builder.HasOne(x => x.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

        }

    }
}
