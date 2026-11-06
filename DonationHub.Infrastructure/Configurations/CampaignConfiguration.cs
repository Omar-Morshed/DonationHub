using System;
using DonationHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Configurations;

public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(cam => cam.RowVersion)
            .IsRowVersion();
        builder.Property(c => c.GoalAmount).HasPrecision(18, 2);
        builder.Property(c => c.RaisedAmount).HasPrecision(18, 2);

        builder.HasOne(cat => cat.Category)
            .WithMany(cam => cam.Campaigns)
            .HasForeignKey(cam => cam.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cam => cam.User)
            .WithMany(user => user.Campaigns)
            .HasForeignKey(cam => cam.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        /* builder.HasMany(cam => cam.Donations)
            .WithOne(don => don.Campaign)
            .HasForeignKey(don => don.CampaignId); */
    }
}
