using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
           builder.HasIndex(x => x.Title).IsUnique();
           builder.HasData(
           new Brand() { BrandId = 1, Title = "Other" },
           new Brand() { BrandId = 2, Title = "None" },

           new Brand() { BrandId = 10, Title = "Toyota" },
           new Brand() { BrandId = 11, Title = "Volkswagen" },
           new Brand() { BrandId = 12, Title = "Ford" },
           new Brand() { BrandId = 13, Title = "Honda" },
           new Brand() { BrandId = 14, Title = "Fiat" },
           new Brand() { BrandId = 15, Title = "BMW" },
           new Brand() { BrandId = 16, Title = "Nissan" },
           new Brand() { BrandId = 17, Title = "Mercedez-Benz" },
           new Brand() { BrandId = 18, Title = "Acura" },
           new Brand() { BrandId = 19, Title = "Alfa-Romeo" },

           new Brand() { BrandId = 20, Title = "Samsung" },
           new Brand() { BrandId = 21, Title = "Apple" },
           new Brand() { BrandId = 22, Title = "Huawei" },
           new Brand() { BrandId = 23, Title = "Xiaomi" },
           new Brand() { BrandId = 24, Title = "Motorola" },
           new Brand() { BrandId = 25, Title = "Lenovo" },
           new Brand() { BrandId = 26, Title = "Nokia" },
           new Brand() { BrandId = 27, Title = "LG" },
           new Brand() { BrandId = 28, Title = "Google Pixel" },
           new Brand() { BrandId = 29, Title = "Asus" },

           new Brand() { BrandId = 30, Title = "Under Armour" },
           new Brand() { BrandId = 31, Title = "Reebok" },
           new Brand() { BrandId = 32, Title = "Adidas" },
           new Brand() { BrandId = 33, Title = "Nike" },
           new Brand() { BrandId = 34, Title = "Converse" },
           new Brand() { BrandId = 35, Title = "Puma" },
           new Brand() { BrandId = 36, Title = "New Balance" },
           new Brand() { BrandId = 37, Title = "Fila" },
           new Brand() { BrandId = 38, Title = "Geox" },
           new Brand() { BrandId = 39, Title = "Vans" },

           new Brand() { BrandId = 40, Title = "ASOS" },
           new Brand() { BrandId = 41, Title = "Tommy Hilfiger" },
           new Brand() { BrandId = 42, Title = "Ralph Lauren" },
           new Brand() { BrandId = 43, Title = "Zara" },
           new Brand() { BrandId = 44, Title = "Levi Strauss" },
           new Brand() { BrandId = 45, Title = "Versace" },
           new Brand() { BrandId = 46, Title = "Hugo Boss" },
           new Brand() { BrandId = 47, Title = "Lacoste" },
           new Brand() { BrandId = 48, Title = "The North Face" },
           new Brand() { BrandId = 49, Title = "Armani" },

           new Brand() { BrandId = 50, Title = "Toshiba" },
           new Brand() { BrandId = 51, Title = "Hewlett-Packard" },
           new Brand() { BrandId = 52, Title = "Sony" },
           new Brand() { BrandId = 53, Title = "Acer" },
           new Brand() { BrandId = 54, Title = "Dell" },
           new Brand() { BrandId = 55, Title = "Microsoft" },
           new Brand() { BrandId = 56, Title = "Msi" },
           new Brand() { BrandId = 57, Title = "Razer" },
           new Brand() { BrandId = 58, Title = "Panasonic" },
           new Brand() { BrandId = 59, Title = "Fujitsu" },

           new Brand() { BrandId = 60, Title = "Omega" },
           new Brand() { BrandId = 61, Title = "Seiko" },
           new Brand() { BrandId = 62, Title = "Casio" },
           new Brand() { BrandId = 63, Title = "Orient" },
           new Brand() { BrandId = 64, Title = "Timex" },
           new Brand() { BrandId = 65, Title = "Fossil" },
           new Brand() { BrandId = 66, Title = "Swatch" },
           new Brand() { BrandId = 67, Title = "Festina" },
           new Brand() { BrandId = 68, Title = "Longines" },
           new Brand() { BrandId = 69, Title = "Citizen" },


           new Brand() { BrandId = 70, Title = "ROCA " },
           new Brand() { BrandId = 71, Title = "Vitra" },
           new Brand() { BrandId = 72, Title = "Ikea" },
           new Brand() { BrandId = 73, Title = "Arper" },
           new Brand() { BrandId = 74, Title = "Knoll" },
           new Brand() { BrandId = 75, Title = "Hay" },
           new Brand() { BrandId = 76, Title = "Moroso" },
           new Brand() { BrandId = 77, Title = "Ekomia" },
           new Brand() { BrandId = 78, Title = "B&B Italia" },
           new Brand() { BrandId = 79, Title = "Minotti" },


           new Brand() { BrandId = 80, Title = "Electrolux" },
           new Brand() { BrandId = 81, Title = "Whirlpool" },
           new Brand() { BrandId = 82, Title = "Vitek" },
           new Brand() { BrandId = 83, Title = "Philips" },
           new Brand() { BrandId = 84, Title = "Gorenje" },
           new Brand() { BrandId = 85, Title = "Gefest" },
           new Brand() { BrandId = 86, Title = "Bosch" },
           new Brand() { BrandId = 87, Title = "Atlant" },
           new Brand() { BrandId = 88, Title = "Rowenta" },
           new Brand() { BrandId = 89, Title = "Kaiser" },

           new Brand() { BrandId = 90, Title = "GoPro" },
           new Brand() { BrandId = 91, Title = "Canon" },
           new Brand() { BrandId = 92, Title = "Benq" },
           new Brand() { BrandId = 93, Title = "Jbl" },
           new Brand() { BrandId = 94, Title = "Sigma" },
           new Brand() { BrandId = 95, Title = "Vesta" },
           new Brand() { BrandId = 96, Title = "Epson" },
           new Brand() { BrandId = 97, Title = "Nikon" },
           new Brand() { BrandId = 98, Title = "Logic" },
           new Brand() { BrandId = 99, Title = "Yamaha" }

           );
        }
    }
}
