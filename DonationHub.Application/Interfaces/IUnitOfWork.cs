using System;

namespace DonationHub.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICampaignRepository CampaignRepository { get; }
    IDonationRepository DonationRepository { get; }
    ICategoryRepository CategoryRepository { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
