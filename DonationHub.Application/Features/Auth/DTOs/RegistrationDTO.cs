namespace DonationHub.Application.Features.Auth.DTOs;

public class RegistrationDTO
{
    public string Email { get; set; } = "";
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PreferredLanguage { get; set; } = "en";
}
