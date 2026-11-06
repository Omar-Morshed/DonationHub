using System;
// using DonationHub.Domain.Enums;

namespace DonationHub.Domain;

public class ApplicationUser
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = "";
    public string Username { get; private set; } = "";
    public string FullName { get; private set; } = "";
    public string PreferredLanguage { get; private set; } = "en";
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    // public string Role { get; set; }

    private ApplicationUser() { }
    public ApplicationUser Create(
        Guid id,
        string email,
        string username,
        string fullName,
        string preferredLanguage,
        DateTime utcNow
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        var lang = preferredLanguage?.ToLowerInvariant() == "en" ? "en" : "ar";

        return new ApplicationUser()
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Email = email,
            Username = username,
            FullName = fullName,
            PreferredLanguage = lang,
            IsActive = true,
            CreatedAt = utcNow
        };
    }
    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
