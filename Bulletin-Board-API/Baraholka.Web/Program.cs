using Baraholka.Data;
using Baraholka.Domain.Models;
using Baraholka.Web.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Baraholka.Web
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
            using (var servicesScope = hostBuilder.Services.CreateScope())
            {
                var services = servicesScope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();
                    var environment = services.GetRequiredService<IWebHostEnvironment>();
                    var imageProcessor = services.GetRequiredService<IImageFileProcessor>();
                    context.Database.Migrate();
                    Seed.SeedUsers(userManager, roleManager);
                    Seed.SeedAnnoucements(context, environment.WebRootPath);
                    Seed.SeedPhotos(context, environment.WebRootPath, imageProcessor);
                }
                catch (Exception e)
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