using Baraholka.Data;
using Baraholka.Data.Seed;
using Baraholka.Domain.Models;
using Baraholka.Services.Services;
using Baraholka.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Baraholka.Web.Infrastructure
{
    public static class WebHostExtensions
    {
        public static IHost SeedData(this IHost hostBuilder)
        {
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
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var filemanager = services.GetRequiredService<IFileManager>();
                    context.Database.Migrate();
                    Seed.SeedUsers(userManager, roleManager);
                    Seed.SeedAnnoucements(context);
                    var imageFolders = configuration.GetSection("AppSettings:ImageFolders").Get<List<ImageFolderConfig>>();
                    Seed.SeedPhotos(context, environment.WebRootPath, imageFolders, imageProcessor, filemanager);
                }
                catch (Exception e)
                {
                    var log = services.GetRequiredService<ILogger<Program>>();
                    log.LogError(e, "Error while migrating");
                }
            }
            return hostBuilder;
        }

        public static IHostBuilder SetLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging(
                (context, logging) =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                    logging.AddConsole();
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                });

            return hostBuilder;
        }
    }
}