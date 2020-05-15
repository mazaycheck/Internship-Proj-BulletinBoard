using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Services;
using Baraholka.Services.Services;
using Baraholka.Utilities;
using Baraholka.Web.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Baraholka.Web
{
    public static class ServicesExtension
    {
        public static void RegisterDependencyInjectionProviders(this IServiceCollection services)
        {
            services.AddScoped<IAnnoucementRepository, AnnoucementRepository>();
            services.AddScoped<IAnnoucementService, AnnoucementService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBrandCategoryService, BrandCategoryService>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IGenericRepository<Annoucement>, GenericRepository<Annoucement>>();
            services.AddScoped<IGenericRepository<Brand>, GenericRepository<Brand>>();
            services.AddScoped<IGenericRepository<BrandCategory>, GenericRepository<BrandCategory>>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IGenericRepository<Message>, GenericRepository<Message>>();
            services.AddScoped<IGenericRepository<Town>, GenericRepository<Town>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IImageFileProcessor, ImageFileProcessor>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPageService<Annoucement>, PageService<Annoucement>>();
            services.AddScoped<IPageService<Brand>, PageService<Brand>>();
            services.AddScoped<IPageService<BrandCategory>, PageService<BrandCategory>>();
            services.AddScoped<IPageService<Category>, PageService<Category>>();
            services.AddScoped<IPageService<Message>, PageService<Message>>();
            services.AddScoped<IPageService<Town>, PageService<Town>>();
            services.AddScoped<IPageService<Town>, PageService<Town>>();
            services.AddScoped<IPageService<User>, PageService<User>>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserAuthorized, CurrentContextUserAuthorized>();
        }
    }
}