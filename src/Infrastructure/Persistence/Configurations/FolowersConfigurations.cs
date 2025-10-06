using Domain.Folowers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FolowersConfiguration : IEntityTypeConfiguration<Folower>
{
    public void Configure(EntityTypeBuilder<Folower> builder)
    {
        builder.HasKey(x => new { x.FollowerId, x.FollowedId });

        builder.Property(x => x.Date)
            .IsRequired();

        builder.HasOne(x => x.Follower)
            .WithMany()
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Followed)
            .WithMany()
            .HasForeignKey(x => x.FollowedId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
