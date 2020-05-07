using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Helpers;
using WebApplication2.Models;
using Microsoft.Extensions.DependencyInjection;
using WebApplication2.Data;
using Microsoft.AspNetCore.Identity;

namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var hostBuilder = CreateHostBuilder(args).ConfigureLogging(
                (context, logging) =>
                {
                    //logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.SetMinimumLevel(LogLevel.Debug);
                    logging.AddConsole();
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                }
                ).Build();
            using(var servicesScope = hostBuilder.Services.CreateScope())
            {
                var services = servicesScope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();
                    var environment = services.GetRequiredService<IWebHostEnvironment>();
                    context.Database.Migrate();
                    Seed.SeedUsers(userManager, roleManager);
                    Seed.SeedAnnoucements(context, environment.WebRootPath);
                }
                catch(Exception e)
                {
                    var log = services.GetRequiredService<ILogger<Program>>();
                    log.LogError(e, "Error while migrating");
                }
            }
            hostBuilder.Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
}
