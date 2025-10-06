using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NickName)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.City)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
