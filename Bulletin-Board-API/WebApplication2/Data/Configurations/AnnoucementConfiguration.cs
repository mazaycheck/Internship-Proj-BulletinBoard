using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models;

namespace WebApplication2.Data.Configurations
{
    public class AnnoucementConfiguration : IEntityTypeConfiguration<Annoucement>
    {        
        public void Configure(EntityTypeBuilder<Annoucement> builder)
        {
            builder.HasMany(p => p.Photos).WithOne(p => p.Annoucement).OnDelete(DeleteBehavior.Cascade); 
        }        
     
    }
}
