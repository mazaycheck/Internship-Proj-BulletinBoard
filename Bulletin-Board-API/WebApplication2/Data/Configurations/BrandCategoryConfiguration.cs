using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class BrandCategoryConfiguration : IEntityTypeConfiguration<BrandCategory>
    {


        public void Configure(EntityTypeBuilder<BrandCategory> builder)
        {
            builder.HasKey(x => x.BrandCategoryId);
            builder.HasAlternateKey(x => new { x.BrandId, x.CategoryId });
            builder.HasOne(x => x.Brand).WithMany(x => x.BrandCategories);
            builder.HasOne(x => x.Category).WithMany(x => x.BrandCategories);

            builder.HasData(

                  new BrandCategory() { BrandCategoryId = 1, CategoryId = 1, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 2, CategoryId = 2, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 3, CategoryId = 3, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 4, CategoryId = 4, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 5, CategoryId = 5, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 6, CategoryId = 6, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 7, CategoryId = 7, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 8, CategoryId = 8, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 9, CategoryId = 9, BrandId = 1 },
                  new BrandCategory() { BrandCategoryId = 10, CategoryId = 10, BrandId = 2 },

                  new BrandCategory() { BrandCategoryId = 100, CategoryId = 1, BrandId = 10 },
                  new BrandCategory() { BrandCategoryId = 101, CategoryId = 1, BrandId = 11 },
                  new BrandCategory() { BrandCategoryId = 102, CategoryId = 1, BrandId = 12 },
                  new BrandCategory() { BrandCategoryId = 103, CategoryId = 1, BrandId = 13 },
                  new BrandCategory() { BrandCategoryId = 104, CategoryId = 1, BrandId = 14 },
                  new BrandCategory() { BrandCategoryId = 105, CategoryId = 1, BrandId = 15 },
                  new BrandCategory() { BrandCategoryId = 106, CategoryId = 1, BrandId = 16 },
                  new BrandCategory() { BrandCategoryId = 107, CategoryId = 1, BrandId = 17 },
                  new BrandCategory() { BrandCategoryId = 108, CategoryId = 1, BrandId = 18 },
                  new BrandCategory() { BrandCategoryId = 109, CategoryId = 1, BrandId = 19 },

                  new BrandCategory() { BrandCategoryId = 200, CategoryId = 2, BrandId = 20 },
                  new BrandCategory() { BrandCategoryId = 201, CategoryId = 2, BrandId = 21 },
                  new BrandCategory() { BrandCategoryId = 202, CategoryId = 2, BrandId = 22 },
                  new BrandCategory() { BrandCategoryId = 203, CategoryId = 2, BrandId = 23 },
                  new BrandCategory() { BrandCategoryId = 204, CategoryId = 2, BrandId = 24 },
                  new BrandCategory() { BrandCategoryId = 205, CategoryId = 2, BrandId = 25 },
                  new BrandCategory() { BrandCategoryId = 206, CategoryId = 2, BrandId = 26 },
                  new BrandCategory() { BrandCategoryId = 207, CategoryId = 2, BrandId = 27 },
                  new BrandCategory() { BrandCategoryId = 208, CategoryId = 2, BrandId = 28 },
                  new BrandCategory() { BrandCategoryId = 209, CategoryId = 2, BrandId = 29 },

                  new BrandCategory() { BrandCategoryId = 300, CategoryId = 3, BrandId = 30 },
                  new BrandCategory() { BrandCategoryId = 301, CategoryId = 3, BrandId = 31 },
                  new BrandCategory() { BrandCategoryId = 302, CategoryId = 3, BrandId = 32 },
                  new BrandCategory() { BrandCategoryId = 303, CategoryId = 3, BrandId = 33 },
                  new BrandCategory() { BrandCategoryId = 304, CategoryId = 3, BrandId = 34 },
                  new BrandCategory() { BrandCategoryId = 305, CategoryId = 3, BrandId = 35 },
                  new BrandCategory() { BrandCategoryId = 306, CategoryId = 3, BrandId = 36 },
                  new BrandCategory() { BrandCategoryId = 307, CategoryId = 3, BrandId = 37 },
                  new BrandCategory() { BrandCategoryId = 308, CategoryId = 3, BrandId = 38 },
                  new BrandCategory() { BrandCategoryId = 309, CategoryId = 3, BrandId = 39 },

                  new BrandCategory() { BrandCategoryId = 400, CategoryId = 4, BrandId = 40 },
                  new BrandCategory() { BrandCategoryId = 401, CategoryId = 4, BrandId = 41 },
                  new BrandCategory() { BrandCategoryId = 402, CategoryId = 4, BrandId = 42 },
                  new BrandCategory() { BrandCategoryId = 403, CategoryId = 4, BrandId = 43 },
                  new BrandCategory() { BrandCategoryId = 404, CategoryId = 4, BrandId = 44 },
                  new BrandCategory() { BrandCategoryId = 405, CategoryId = 4, BrandId = 45 },
                  new BrandCategory() { BrandCategoryId = 406, CategoryId = 4, BrandId = 46 },
                  new BrandCategory() { BrandCategoryId = 407, CategoryId = 4, BrandId = 47 },
                  new BrandCategory() { BrandCategoryId = 408, CategoryId = 4, BrandId = 48 },
                  new BrandCategory() { BrandCategoryId = 409, CategoryId = 4, BrandId = 49 },

                  new BrandCategory() { BrandCategoryId = 500, CategoryId = 5, BrandId = 50 },
                  new BrandCategory() { BrandCategoryId = 501, CategoryId = 5, BrandId = 51 },
                  new BrandCategory() { BrandCategoryId = 502, CategoryId = 5, BrandId = 52 },
                  new BrandCategory() { BrandCategoryId = 503, CategoryId = 5, BrandId = 53 },
                  new BrandCategory() { BrandCategoryId = 504, CategoryId = 5, BrandId = 54 },
                  new BrandCategory() { BrandCategoryId = 505, CategoryId = 5, BrandId = 55 },
                  new BrandCategory() { BrandCategoryId = 506, CategoryId = 5, BrandId = 56 },
                  new BrandCategory() { BrandCategoryId = 507, CategoryId = 5, BrandId = 57 },
                  new BrandCategory() { BrandCategoryId = 508, CategoryId = 5, BrandId = 58 },
                  new BrandCategory() { BrandCategoryId = 509, CategoryId = 5, BrandId = 59 },

                  new BrandCategory() { BrandCategoryId = 600, CategoryId = 6, BrandId = 60 },
                  new BrandCategory() { BrandCategoryId = 601, CategoryId = 6, BrandId = 61 },
                  new BrandCategory() { BrandCategoryId = 602, CategoryId = 6, BrandId = 62 },
                  new BrandCategory() { BrandCategoryId = 603, CategoryId = 6, BrandId = 63 },
                  new BrandCategory() { BrandCategoryId = 604, CategoryId = 6, BrandId = 64 },
                  new BrandCategory() { BrandCategoryId = 605, CategoryId = 6, BrandId = 65 },
                  new BrandCategory() { BrandCategoryId = 606, CategoryId = 6, BrandId = 66 },
                  new BrandCategory() { BrandCategoryId = 607, CategoryId = 6, BrandId = 67 },
                  new BrandCategory() { BrandCategoryId = 608, CategoryId = 6, BrandId = 68 },
                  new BrandCategory() { BrandCategoryId = 609, CategoryId = 6, BrandId = 69 },

                  new BrandCategory() { BrandCategoryId = 700, CategoryId = 7, BrandId = 70 },
                  new BrandCategory() { BrandCategoryId = 701, CategoryId = 7, BrandId = 71 },
                  new BrandCategory() { BrandCategoryId = 702, CategoryId = 7, BrandId = 72 },
                  new BrandCategory() { BrandCategoryId = 703, CategoryId = 7, BrandId = 73 },
                  new BrandCategory() { BrandCategoryId = 704, CategoryId = 7, BrandId = 74 },
                  new BrandCategory() { BrandCategoryId = 705, CategoryId = 7, BrandId = 75 },
                  new BrandCategory() { BrandCategoryId = 706, CategoryId = 7, BrandId = 76 },
                  new BrandCategory() { BrandCategoryId = 707, CategoryId = 7, BrandId = 77 },
                  new BrandCategory() { BrandCategoryId = 708, CategoryId = 7, BrandId = 78 },
                  new BrandCategory() { BrandCategoryId = 709, CategoryId = 7, BrandId = 79 },

                  new BrandCategory() { BrandCategoryId = 800, CategoryId = 8, BrandId = 80 },
                  new BrandCategory() { BrandCategoryId = 801, CategoryId = 8, BrandId = 81 },
                  new BrandCategory() { BrandCategoryId = 802, CategoryId = 8, BrandId = 82 },
                  new BrandCategory() { BrandCategoryId = 803, CategoryId = 8, BrandId = 83 },
                  new BrandCategory() { BrandCategoryId = 804, CategoryId = 8, BrandId = 84 },
                  new BrandCategory() { BrandCategoryId = 805, CategoryId = 8, BrandId = 85 },
                  new BrandCategory() { BrandCategoryId = 806, CategoryId = 8, BrandId = 86 },
                  new BrandCategory() { BrandCategoryId = 807, CategoryId = 8, BrandId = 87 },
                  new BrandCategory() { BrandCategoryId = 808, CategoryId = 8, BrandId = 88 },
                  new BrandCategory() { BrandCategoryId = 809, CategoryId = 8, BrandId = 89 },

                  new BrandCategory() { BrandCategoryId = 900, CategoryId = 9, BrandId = 90 },
                  new BrandCategory() { BrandCategoryId = 901, CategoryId = 9, BrandId = 91 },
                  new BrandCategory() { BrandCategoryId = 902, CategoryId = 9, BrandId = 92 },
                  new BrandCategory() { BrandCategoryId = 903, CategoryId = 9, BrandId = 93 },
                  new BrandCategory() { BrandCategoryId = 904, CategoryId = 9, BrandId = 94 },
                  new BrandCategory() { BrandCategoryId = 905, CategoryId = 9, BrandId = 95 },
                  new BrandCategory() { BrandCategoryId = 906, CategoryId = 9, BrandId = 96 },
                  new BrandCategory() { BrandCategoryId = 907, CategoryId = 9, BrandId = 97 },
                  new BrandCategory() { BrandCategoryId = 908, CategoryId = 9, BrandId = 98 },
                  new BrandCategory() { BrandCategoryId = 909, CategoryId = 9, BrandId = 99 }
                  

                    );

        }
    }
}
