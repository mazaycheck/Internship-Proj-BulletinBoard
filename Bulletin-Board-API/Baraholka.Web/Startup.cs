using AutoMapper;
using Baraholka.Data;
using Baraholka.Data.Configurations;
using Baraholka.Utilities;
using Baraholka.Web.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRChat.Hubs;
using System.Collections.Generic;

namespace Baraholka.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(
                typeof(AutoMapperConfig).Assembly,
                typeof(ServiceMappingProfile).Assembly);

            services.AddHttpContextAccessor();

            services.Configure<List<ImageFolderConfig>>(Configuration.GetSection("AppSettings:ImageFolders"));

            services.RegisterDependencies();

            services.ConfigureIdentity();

            services.ConfigureControllers();

            services.ConfigureAuthentication(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administration", policy => policy.RequireRole("Admin", "Moderator"));
            });

            services.ConfigureCors();

            services.ConfigureSwagger();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandling(env);

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Baraholka API");
            });
        }
    }
}