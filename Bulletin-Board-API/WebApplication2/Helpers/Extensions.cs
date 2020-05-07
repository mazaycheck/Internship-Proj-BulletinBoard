using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Helpers
{

    //public class DataSeedingContext : DbContext
    //{
    //    private readonly DbContextOptions<DataSeedingContext> options;

    //    public DataSeedingContext(DbContextOptions<DataSeedingContext> options) : base(options)
    //    {
    //        this.options = options;
    //    }

    //    public DbSet<User> Users { get; set; }
    //}


    //public static class MyExtensions
    //{
    //    public static IWebHost SeedData(this IWebHost host)
    //    {
    //        using (var scope = host.Services.CreateScope())
    //        {
    //            var serviceProvider = scope.ServiceProvider;
    //            var context = serviceProvider.GetService<DataSeedingContext>();
    //            Helpers.SeedUsers(context);
    //        }
    //        return host;
    //    }

    //}

    //public class Helpers
    //{
    //    public static void SeedUsers(DataSeedingContext context)
    //    {
    //        User[] users =
    //        {
    //            new User(){ Name = "Jorik", Email = "jorik@mail.ru", PhoneNumber = "213123213"},
    //            new User(){ Name = "Boris", Email = "boris@yahoo.ru", PhoneNumber = "83123213"},
    //            new User(){ Name = "Leonid", Email = "leonea23@google.ru", PhoneNumber = "34342222"},
    //        };
    //        context.AttachRange(users);
    //        context.SaveChanges();
    //    }
    //}

}
