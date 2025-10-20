using Domain.Travels;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TravelsConfiguration : IEntityTypeConfiguration<Travel>
{
    public void Configure(EntityTypeBuilder<Travel> builder)
    {
        builder.HasKey(x => x.Id);

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
    }
}
