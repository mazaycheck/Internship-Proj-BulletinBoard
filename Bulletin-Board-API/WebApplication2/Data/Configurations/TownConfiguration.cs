using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.HasIndex(x => x.Title).IsUnique();
            builder.HasData(
                new Town() { TownId = 1, Title = "Balti", CoordX = 250, CoordY = 300 },
                new Town() { TownId = 2, Title = "Briceni", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 3, Title = "Cahul", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 4, Title = "Camenca", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 5, Title = "Camtemir", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 6, Title = "Calarasi", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 7, Title = "Comrat", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 8, Title = "Cricova", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 9, Title = "Chisinau", CoordX = 199, CoordY = 250 },
                new Town() { TownId = 10, Title = "Drochia", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 11, Title = "Dubasar", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 12, Title = "Durlesti", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 13, Title = "Edinet", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 14, Title = "Falesti", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 15, Title = "Glodeni", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 16, Title = "Hincesti", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 17, Title = "Ialoveni", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 18, Title = "Leova", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 19, Title = "Lipcani", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 20, Title = "Nisporeni", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 21, Title = "Orhei", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 22, Title = "Ocnita", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 23, Title = "Rezina", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 24, Title = "Riscani", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 25, Title = "Soroca", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 26, Title = "Straseni", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 27, Title = "Taraclia", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 28, Title = "Tighina", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 29, Title = "Tiraspol", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 30, Title = "Ungheni", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 31, Title = "Vatra", CoordX = 100, CoordY = 115 },
                new Town() { TownId = 32, Title = "Vulcanesti", CoordX = 100, CoordY = 115 }
            );

        }
    }
}
