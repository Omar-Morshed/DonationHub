using System;
using System.Security.AccessControl;
using DonationHub.Application.Interfaces;
using DonationHub.Domain;

namespace DonationHub.Infrastructure.Implementation;

public class DonationRepository : GenericRepository<Donation>, IDonationRepository
{
    public DonationRepository(AppDbContext context) : base(context)
    {
    }
}
