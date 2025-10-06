using Domain.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PlacesConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Latitude)
            .IsRequired();

        builder.Property(x => x.Longitude)
            .IsRequired();

        builder.HasMany(x => x.PlacePhotos)
            .WithOne(x => x.Place)
            .HasForeignKey(x => x.PlaceId);
    }
}
