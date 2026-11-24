using System;

namespace DonationHub.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICampaignRepository CampaignRepository { get; }
    IDonationRepository DonationRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IUserRepository UserRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
