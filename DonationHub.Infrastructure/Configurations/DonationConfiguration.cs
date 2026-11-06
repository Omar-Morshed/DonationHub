using System;
using DonationHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Configurations;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.HasOne(don => don.User)
            .WithMany(user => user.Donations)
            .HasForeignKey(don => don.DonorUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Campaign)
            .WithMany(c => c.Donations)
            .HasForeignKey(d => d.CampaignId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(d => d.Amount).HasPrecision(18, 2);
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}
