using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new UserId(x));

            builder.Property(x => x.NickName)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(x => x.Avatar)
                .HasColumnType("bytea")
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .HasConversion(new DateTimeUtcConverter())
                .HasDefaultValueSql("timezone('utc', now())")
                .IsRequired();

            builder.Property(x => x.RoleId)
                .IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.SetNull);

            // Колекція подорожей, створених користувачем
            builder.HasMany(x => x.Travels)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Колекція участі користувача в подорожах
            builder
                .Navigation(x => x.UserTravels)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
