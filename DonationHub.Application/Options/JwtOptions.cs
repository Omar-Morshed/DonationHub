using System;
using System.ComponentModel.DataAnnotations;

namespace DonationHub.Application.Options;

public class JwtOptions
{
    [Required]
    [MinLength(32, ErrorMessage = "The Secret Key should contains 32 bytes / characters minimum")]
    public string SecretKey { get; init; } = null!;

    [Required(ErrorMessage = "The Issuer is required")]
    public string Issuer { get; init; } = null!;

    [Required(ErrorMessage = "The Audience is required")]
    public string Audience { get; init; } = null!;

    [Range(1, 1440, ErrorMessage = "ExpiryMinutes must be between 1 minute and 1 day (1440 mins).")]
    public int ExpiryMinutes { get; init; }

    [Range(1, 365, ErrorMessage = "RefreshTokenExpiryDays must be between 1 and 365 days.")]
    public int RefreshTokenExpiryDays { get; init; }
}
