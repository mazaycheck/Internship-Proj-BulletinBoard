using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            
            //builder.Property(x => x.RegistrationDate).HasDefaultValueSql($"{DateTime.Now}");

            //builder.HasData(
            //    new User() { UserId = 1, Name = "Vasea", Email = "Vasea@gmail.com", PhoneNumber = "06546876", TownId = 1,  RegistrationDate = DateTime.Now - TimeSpan.FromDays(30), Password = Encoding.UTF8.GetBytes("Password123") , PasswordSalt = Encoding.UTF8.GetBytes("Salt")},
            //    new User() { UserId = 2, Name = "Olga", Email = "Olga@gmail.com", PhoneNumber = "06546876", TownId = 2 , RegistrationDate = DateTime.Now - TimeSpan.FromDays(15), Password = Encoding.UTF8.GetBytes("Password123") , PasswordSalt = Encoding.UTF8.GetBytes("Salt") },
            //    new User() { UserId = 3, Name = "Iura", Email = "Iura@gmail.com", PhoneNumber = "06546876", TownId = 3, RegistrationDate = DateTime.Now - TimeSpan.FromDays(50), Password = Encoding.UTF8.GetBytes("Password123") , PasswordSalt = Encoding.UTF8.GetBytes("Salt") }
            //    );
        }
    }
}
