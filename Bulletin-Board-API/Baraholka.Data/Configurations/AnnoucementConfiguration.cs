using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Baraholka.Domain.Models;

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