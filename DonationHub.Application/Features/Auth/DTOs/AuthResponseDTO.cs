namespace DonationHub.Application.Features.Auth.DTOs;

public record AuthResponseDTO(
    string AccessToken,
    DateTime ExpiresAt,
    string RefreshToken,
    Guid UserId,
    string Email,
    string FullName,
    string PreferredLanguage,
    IReadOnlyList<string> Roles
);