using Baraholka.Web.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Baraholka.Web
{
    public class Program
    {
        public static void Main(string[] args) =>

            CreateHostBuilder(args)
                .SetLogging()
                .Build()
                .SeedData()
                .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder => webBuilder
                        .UseStartup<Startup>());
    }
}