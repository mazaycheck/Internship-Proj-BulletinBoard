using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SignalRChat.Hubs;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Data.Repositories;
using WebApplication2.Helpers;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2
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
            services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<Annoucement>, GenericRepository<Annoucement>>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IGenericRepository<Town>, GenericRepository<Town>>();
            services.AddScoped<IGenericRepository<Brand>, GenericRepository<Brand>>();
            services.AddScoped<IGenericRepository<BrandCategory>, GenericRepository<BrandCategory>>();
            services.AddScoped<IGenericRepository<Message>, GenericRepository<Message>>();
            services.AddScoped<IAnnoucementRepository, AnnoucementRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IPageService<Annoucement>, PageService<Annoucement>>();
            services.AddScoped<IPageService<Category>, PageService<Category>>();
            services.AddScoped<IPageService<Town>, PageService<Town>>();
            services.AddScoped<IPageService<Brand>, PageService<Brand>>();
            services.AddScoped<IPageService<BrandCategory>, PageService<BrandCategory>>();
            services.AddScoped<IPageService<Town>, PageService<Town>>();
            services.AddScoped<IPageService<Message>, PageService<Message>>();
            services.AddScoped<IPageService<User>, PageService<User>>();
            services.AddAutoMapper(typeof(Annoucement).Assembly);
            services.AddScoped<IAnnoucementService, AnnoucementService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IBrandCategoryService, BrandCategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IImageFileProcessor, ImageFileProcessor>();
            services.AddHttpContextAccessor();

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
                                        var path = context.HttpContext.Request.Path;
                                        if (!string.IsNullOrEmpty(accessToken) &&
                                            (path.StartsWithSegments("/chatHub")))
                                        {
                                            context.Token = accessToken;
                                        }
                                        return Task.CompletedTask;
                                    }
                                };
                            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administration", policy => policy.RequireRole("Admin", "Moderator"));
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(config => config.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var erMessage = context.Features.Get<IExceptionHandlerFeature>().Error.Message;
                    await context.Response.WriteAsync(erMessage);
                }));
            }
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
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Baraholka API"); });
        }
    }
}