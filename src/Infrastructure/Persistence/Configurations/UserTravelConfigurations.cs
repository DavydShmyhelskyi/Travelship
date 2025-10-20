using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserTravelConfigurations : IEntityTypeConfiguration<Domain.Users.UserTravel>   
    {
        public void Configure(EntityTypeBuilder<Domain.Users.UserTravel> builder)
        {
            builder.HasKey(ut => new { ut.UserId, ut.TravelId });

            builder.Property(ut => ut.UserId)
                .HasConversion(x => x.Value, x => new Domain.Users.UserId(x));
            builder.HasOne<Domain.Users.User>()
                .WithMany(u => u.Travels)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ut => ut.TravelId)
                .HasConversion(x => x.Value, x => new Domain.Travels.TravelId(x));
            builder.HasOne<Domain.Travels.Travel>()
                .WithMany(t => t.Members)
                .HasForeignKey(ut => ut.TravelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
