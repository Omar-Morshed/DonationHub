using DonationHub.Application.Interfaces;
using DonationHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Implementation;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    { }

    public async Task<RefreshToken?> GetByTokenHash(string tokenHash)
    {
        var refreshToken = await _dbSet.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        return refreshToken;
    }

}
