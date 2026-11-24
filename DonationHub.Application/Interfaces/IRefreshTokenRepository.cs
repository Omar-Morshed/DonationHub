using DonationHub.Domain;

namespace DonationHub.Application.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenHash(string tokenHash);
}
