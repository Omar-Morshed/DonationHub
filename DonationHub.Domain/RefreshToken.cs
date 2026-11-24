using System;

namespace DonationHub.Domain;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime CreateAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByTokenId { get; private set; } //* Used for Token Rotation (making new token when the existing one is expired), and Reuse Detection for the hackers
    private RefreshToken()
    { }

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTime createAt,
        DateTime expiresAt
    )
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenHash,
            CreateAt = createAt,
            ExpiresAt = expiresAt,
        };
    }

    public bool Revoke(DateTime utcNow)
    {
        // لو ملغي قبل كده أو وقت الإلغاء غير منطقي
        if (RevokedAt is not null || utcNow < CreateAt)
            return false;

        RevokedAt = utcNow;
        return true;
    }
}
