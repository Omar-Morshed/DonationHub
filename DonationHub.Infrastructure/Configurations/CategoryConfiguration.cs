using System;
using DonationHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        /* builder.HasMany(category => category.Campaigns).WithOne(campaign => campaign.Category); */
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}
