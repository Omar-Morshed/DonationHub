using System;
using DonationHub.Application.Interfaces;

namespace DonationHub.Infrastructure.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ICampaignRepository CampaignRepository { get; private set; }

    public IDonationRepository DonationRepository { get; private set; }

    public ICategoryRepository CategoryRepository { get; private set; }
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        CampaignRepository = new CampaignRepository(_context);
        CategoryRepository = new CategoryRepository(_context);
        DonationRepository = new DonationRepository(_context);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
