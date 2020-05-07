using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication2.Data;
using WebApplication2.Data.Repositories;
using WebApplication2.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using WebApplication2.Models;
using Microsoft.OpenApi.Models;
using WebApplication2.MyMiddleware;
using AutoMapper;

using SignalRChat.Hubs;
using WebApplication2.hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace WebApplication2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();

            services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                //.UseSqlite(Configuration.GetConnectionString("BackupConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAnnoucementRepository, AnnoucementRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IGenericRepository<Town>, GenericRepository<Town>>();
            services.AddScoped<IGenericRepository<Brand>, GenericRepository<Brand>>();
            services.AddScoped<IGenericRepository<BrandCategory>, GenericRepository<BrandCategory>>();
            services.AddScoped<IAnnoucementService, AnnoucementService>();
            services.AddAutoMapper(typeof(Annoucement).Assembly);
            services.AddScoped<IGenericRepository<Message>, GenericRepository<Message>>();
            services.AddScoped<IMessageService, MessageService>();

            IdentityBuilder identityBuilder = services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(Role), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<AppDbContext>();
            identityBuilder.AddRoleValidator<RoleValidator<Role>>();
            identityBuilder.AddRoleManager<RoleManager<Role>>();
            identityBuilder.AddSignInManager<SignInManager<User>>();





            services.AddControllers(options => 
            {
                var policyBuilder = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policyBuilder));
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling =
                    Newtonsoft.Json.NullValueHandling.Ignore;
                });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters()
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                    ValidateAudience = false,
                                    ValidateIssuer = false,
                                };
                                options.SaveToken = true;
                                options.Events = new JwtBearerEvents
                                {
                                    OnMessageReceived = context =>
                                    {
                                        var accessToken = context.Request.Query["access_token"];

                                        // If the request is for our hub...
                                        var path = context.HttpContext.Request.Path;
                                        if (!string.IsNullOrEmpty(accessToken) &&
                                            (path.StartsWithSegments("/chatHub")))
                                        {
                                            // Read the token out of the query string
                                            context.Token = accessToken;
                                        }
                                        return Task.CompletedTask;
                                    }
                                };

                            });


            services.AddAuthorization(options => 
            {
                options.AddPolicy("ReqAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ReqModOrAdmin", policy => policy.RequireRole("Admin", "Moderator"));
                options.AddPolicy("AllowOnlyMembers", policy => policy.RequireRole("Member"));
                options.AddPolicy("AllowAllLoggedIn", policy => policy.RequireAuthenticatedUser());
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            services.AddSwaggerGen(options => { options.SwaggerDoc("v1", new OpenApiInfo { Title = "Baraholka API", Version = "v1" }); });
            services.AddSignalR();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(config => config.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var erMessage = context.Features.Get<IExceptionHandlerFeature>().Error.Message;
                    await context.Response.WriteAsync(erMessage);
                }));    

            }
            
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseAuthentication();
         
            app.Use(next =>
            {
                
                return context =>
                {
                    var t = context;
                    return next(context);
                };
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Baraholka API"); });

        }
    }
}
