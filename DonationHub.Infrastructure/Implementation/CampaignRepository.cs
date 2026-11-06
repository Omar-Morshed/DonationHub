using System;
using DonationHub.Application.Interfaces;
using DonationHub.Domain;

namespace DonationHub.Infrastructure.Implementation;

public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
{
    public CampaignRepository(AppDbContext context) : base(context)
    {
    }
}
