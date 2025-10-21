using Domain.Followers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FollowersConfiguration : IEntityTypeConfiguration<Follower>
{
    public void Configure(EntityTypeBuilder<Follower> builder)
    {
        builder.HasKey(x => new { x.FollowerUserId, x.FollowedUserId });

        builder.Property(x => x.Date)
            .IsRequired();

        builder.HasOne(x => x.FollowerUser)
            .WithMany() 
            .HasForeignKey(x => x.FollowerUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FollowedUser)
            .WithMany()
            .HasForeignKey(x => x.FollowedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
