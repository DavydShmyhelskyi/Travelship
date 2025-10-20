using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Places;
using Domain.Travels;

namespace Infrastructure.Persistence.Configurations
{
    public class TravelPlaceConfigurations : IEntityTypeConfiguration<TravelPlace>
    {
        public void Configure(EntityTypeBuilder<TravelPlace> builder)
        {
            builder.HasKey(tp => new { tp.TravelId, tp.PlaceId });

            builder.Property(x => x.TravelId)
                .HasConversion(id => id.Value, x => new TravelId(x));
            builder.HasOne(x => x.travel)
                .WithMany(t => t.Places)
                .HasForeignKey(tp => tp.TravelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.PlaceId)
                .HasConversion(id => id.Value, x => new Domain.Places.PlaceId(x));
            builder.HasOne(x => x.place)
                .WithMany(p => p.Travels)
                .HasForeignKey(tp => tp.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
