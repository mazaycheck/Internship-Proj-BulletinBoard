using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Configurations;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore.Proxies;
using WebApplication2.Models.Annoucements;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int, 
        IdentityUserClaim<int>, UserRole, 
        IdentityUserLogin<int>, IdentityRoleClaim<int>, 
        IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //base.OnConfiguring(optionsBuilder.UseLazyLoadingProxies());         
            base.OnConfiguring(optionsBuilder.EnableSensitiveDataLogging());
         
    }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfiguration<UserRole>(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration<User>(new UserConfiguration());
            modelBuilder.ApplyConfiguration<Town>(new TownConfiguration());

            modelBuilder.ApplyConfiguration<Brand>(new BrandConfiguration());
            modelBuilder.ApplyConfiguration<Category>(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration<BrandCategory>(new BrandCategoryConfiguration());

            modelBuilder.ApplyConfiguration<Annoucement>(new AnnoucementConfiguration());
            modelBuilder.ApplyConfiguration<VehicleAnnoucement>(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration<Message>(new MessageConfiguration());        
            modelBuilder.ApplyConfiguration<UserProfile>(new UserProfileConfiguration());

            modelBuilder.Entity<User>();
            modelBuilder.Entity<VehicleAnnoucement>();
            modelBuilder.Entity<ElectronicsAnnoucement>();
            modelBuilder.Entity<ClothesAnnoucement>();

        }

        
        //public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BrandCategory> BrandCategories { get; set; }
        public DbSet<Annoucement> Annoucements { get; set; }        
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }             
        //public DbSet<Chat> Chats { get; set; }
        //public DbSet<MessageSent> MessagesSent { get; set; }
        //public DbSet<MessageRecieved> MessagesRecieved { get; set; }
        
    }
}
