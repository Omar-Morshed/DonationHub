namespace DonationHub.Application.Features.Admin.DTOs;

public record CreateStaffRequestDTO(
    string FullName,
    string Email,
    string Password,
    string PreferredLanguage,
    string Role
);
