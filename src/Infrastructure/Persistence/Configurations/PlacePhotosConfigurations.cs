using Domain.PlacePhotos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PlacePhotosConfiguration : IEntityTypeConfiguration<PlacePhoto>
{
    public void Configure(EntityTypeBuilder<PlacePhoto> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Photo)
            .HasColumnType("bytea")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.IsShown)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasOne(x => x.Place)
            .WithMany(x => x.PlacePhotos)
            .HasForeignKey(x => x.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
