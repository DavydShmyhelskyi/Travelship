using Domain.Travels;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TravelsConfiguration : IEntityTypeConfiguration<Travel>
    {
        public void Configure(EntityTypeBuilder<Travel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(builder => builder.Id)
                .HasConversion(x => x.Value, x => new TravelId(x));

            builder.Property(x => x.Title)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("varchar(1000)")
                .IsRequired();

            builder.Property(x => x.Image)
                .HasColumnType("bytea")
                .IsRequired(false);

            builder.Property(x => x.StartDate)
                .HasConversion(new DateTimeUtcConverter())
                .IsRequired();

            builder.Property(x => x.EndDate)
                .HasConversion(new DateTimeUtcConverter())
                .IsRequired();

            builder.Property(x => x.IsDone)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Travels)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Navigation settings for private fields ---
            builder.Navigation(x => x.Members)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(x => x.Places)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
