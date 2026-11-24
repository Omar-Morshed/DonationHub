using DonationHub.Domain;
using Microsoft.AspNetCore.Identity;

namespace DonationHub.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public User DomainUser { get; set; } = null!;
}
