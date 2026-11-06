using System;
using System.ComponentModel.DataAnnotations;

namespace DonationHub.Application.Options;

public class JwtOptions
{
    [Required]
    [MinLength(32, ErrorMessage = "The Secret Key should contains 32 bytes / characters minimum")]
    public string SecretKey { get; init; } = null!;
    [Required]
    public string Issuer { get; init; } = null!;
    [Required]
    public string Audience { get; init; } = null!;
    public double ExpiryMinutes { get; init; }
    public DateTime RefreshTokenExpiryDays { get; init; }
}
