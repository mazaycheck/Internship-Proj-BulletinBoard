using Baraholka.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baraholka.Data.Configurations
{
    public class AnnoucementConfiguration : IEntityTypeConfiguration<Annoucement>
    {
        public void Configure(EntityTypeBuilder<Annoucement> builder)
        {
            builder.HasMany(p => p.Photos).WithOne(p => p.Annoucement).OnDelete(DeleteBehavior.Cascade);
        }
    }
}