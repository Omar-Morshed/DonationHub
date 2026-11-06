namespace DonationHub.Domain;

public record IdempotencyRecord(
    Guid Key,
    Guid UserId,
    string ResponseStatusCode,
    string ResponseBody,
    string RequestHash,
    DateTime CreatedAt,
    DateTime ExpiresAt
);
