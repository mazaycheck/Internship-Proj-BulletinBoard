using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Models.Annoucements;



namespace WebApplication2.Data.Configurations
{
    public class AnnoucementConfiguration : IEntityTypeConfiguration<Annoucement>
    {

        

        public void Configure(EntityTypeBuilder<Annoucement> builder)
        {

            builder.HasMany(p => p.Photos).WithOne(p => p.Annoucement).OnDelete(DeleteBehavior.Cascade);




            //builder.HasData(
            //    new Annoucement() { AnnoucementId = 1, Title = "New Honda car", Description = "Brand new Honda", CreateDate = DateTime.Now, Price = 5000, UserId = 1, BrandCategoryId = 4 },
            //    new Annoucement() { AnnoucementId = 2, Title = "Iphone 6", Description = "Used phone", CreateDate = DateTime.Now, Price = 250, UserId = 2, BrandCategoryId = 5 },
            //    new Annoucement() { AnnoucementId = 3, Title = "Adidas Sneackers", Description = "From 2020 Collection", CreateDate = DateTime.Now, Price = 5000, UserId = 3, BrandCategoryId = 6 },
            //    new Annoucement() { AnnoucementId = 4, Title = "Nokia 1100", Description = "Old School phone", CreateDate = DateTime.Now - TimeSpan.FromDays(10), Price = 50, UserId = 3, BrandCategoryId = 5 },
            //    new Annoucement() { AnnoucementId = 5, Title = "Reebok SportSuit", Description = "From 2019 Collection", CreateDate = DateTime.Now, Price = 100, UserId = 1, BrandCategoryId = 6 },
            //    new Annoucement() { AnnoucementId = 6, Title = "2010 Toyota Corolla", Description = "Comfortable car", CreateDate = DateTime.Now, Price = 6000, UserId = 2, BrandCategoryId = 7  }
            //    );

 
        }        
        public void Configure(EntityTypeBuilder<VehicleAnnoucement> builder)
        {
            //builder.HasData(
            //    new VehicleAnnoucement() { AnnoucementId = 7, Title = "New Honda car", Description = "Brand new BMW", CreateDate = DateTime.Now, Price = 5000, UserId = 1, BrandCategory = brandCategories[4] },
            //    new VehicleAnnoucement() { AnnoucementId = 8, Title = "2010 Toyota Corolla", Description = "Comfortable car", CreateDate = DateTime.Now, Price = 6000, UserId = 2, BrandCategory = brandCategories[5] }
            //    );
        }
    }
}
