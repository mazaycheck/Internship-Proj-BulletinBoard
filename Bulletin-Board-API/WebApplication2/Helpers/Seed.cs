using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Data.SeedData;
using WebApplication2.Models;


namespace WebApplication2.Helpers
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {


            var roles = new List<Role>()
            {
                new Role{Name= "Admin" },
                new Role{Name = "Moderator"},
                new Role{Name = "Member"},
                new Role{Name= "Hacker" },
            };

            foreach (var role in roles)
            {
                roleManager.CreateAsync(role).Wait();
            }

            if (!userManager.Users.Any())
            {
                var userJsonData = System.IO.File.ReadAllText("Data/SeedData/users.json");                
                var format = "dd-MM-yyyy";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                var users = JsonConvert.DeserializeObject<List<User>>(userJsonData, dateTimeConverter);
                foreach (var user in users)
                {

                    userManager.CreateAsync(user, "password123").Wait();
                    userManager.AddToRoleAsync(user, "Member").Wait();
                }

                //For testing only
                var admin = new User() { UserName = "Admin", Email = "admin@myweb.com" };                
                userManager.CreateAsync(admin, "password123").Wait();
                userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator"}).Wait();
            }         
        }

        public static void SeedAnnoucements(AppDbContext context, string rootpath)
        {
            if (!context.Annoucements.Any()) { 
            AnnoucementData.RootPath = rootpath;
            foreach(var item in AnnoucementData.GetData(10))
            {
                context.Annoucements.Add(item);
            }

            context.Database.OpenConnection();
            try
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Annoucements ON");                
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Annoucements OFF");
            }
            finally
            {
                context.Database.CloseConnection();
            }
            context.SaveChanges();
            }
        }
    }
}
