namespace DonationHub.Application.Features.Auth.DTOs;

public record UserProfileDTO(
    Guid Id,
    string Email,
    string FullName,
    string PreferredLanguage,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<string> Roles
);
