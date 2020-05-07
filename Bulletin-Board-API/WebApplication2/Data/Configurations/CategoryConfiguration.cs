using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasIndex(x => x.Title).IsUnique();
            builder.HasData(
                new Category() { CategoryId = 1, Title = "Vehicles"},
                new Category() { CategoryId = 2, Title = "Mobile Phones"},
                new Category() { CategoryId = 3, Title = "Shoes"},
                new Category() { CategoryId = 4, Title = "Clothes"},
                new Category() { CategoryId = 5, Title = "Computers"},
                new Category() { CategoryId = 6, Title = "Watches"},
                new Category() { CategoryId = 7, Title = "Furniture"},
                new Category() { CategoryId = 8, Title = "Appliances"},
                new Category() { CategoryId = 9, Title = "Audio/Video"},
                new Category() { CategoryId = 10, Title = "Services"}
                );
        }
    }
}
