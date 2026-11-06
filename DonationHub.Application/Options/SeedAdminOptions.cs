using System;
using System.ComponentModel.DataAnnotations;

namespace DonationHub.Application.Options;

public class SeedAdminOptions
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
