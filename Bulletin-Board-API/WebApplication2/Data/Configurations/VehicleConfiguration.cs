using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models.Annoucements;

namespace WebApplication2.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<VehicleAnnoucement>
    {



        public void Configure(EntityTypeBuilder<VehicleAnnoucement> builder)
        {
           // builder.HasData(
           //new VehicleAnnoucement() { AnnoucementId = 7, Title = "New Honda car", Description = "Brand new BMW", CreateDate = DateTime.Now, Price = 5000, UserId = 1, BrandCategoryId = 5},
           //new VehicleAnnoucement() { AnnoucementId = 8, Title = "2010 Toyota Corolla", Description = "Comfortable car", CreateDate = DateTime.Now, Price = 6000, UserId = 2, BrandCategoryId = 2 }
           //);
        }
    }
}