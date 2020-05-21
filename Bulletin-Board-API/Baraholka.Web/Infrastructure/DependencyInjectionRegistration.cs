using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Services;
using Baraholka.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Baraholka.Web.Infrastructure
{
    public static class DependencyInjectionRegistration
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAnnoucementRepository, AnnoucementRepository>();
            services.AddScoped<IAnnoucementService, AnnoucementService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBrandCategoryService, BrandCategoryService>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IBrandCategoryRepository, BrandCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITownRepository, TownRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IGenericRepository<Annoucement>, GenericRepository<Annoucement>>();
            services.AddScoped<IGenericRepository<Brand>, GenericRepository<Brand>>();
            services.AddScoped<IGenericRepository<BrandCategory>, GenericRepository<BrandCategory>>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IGenericRepository<Message>, GenericRepository<Message>>();
            services.AddScoped<IGenericRepository<Town>, GenericRepository<Town>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IImageFileProcessor, ImageFileProcessor>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageFileManager, ImageFileManager>();
            services.AddScoped<IImageFolderConfigAccessor, ImageFolderConfigAccessor>();
            services.AddScoped<IRootPathProvider, RootPathProvider>();
        }
    }
}