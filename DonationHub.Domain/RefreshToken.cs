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
}
