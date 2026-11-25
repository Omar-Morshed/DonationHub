using DonationHub.Domain;
using DonationHub.Domain.Enums;
using DonationHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static DonationHub.Domain.User;

namespace DonationHub.Infrastructure.Persistence.Seeders;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        // // string[] roles = ["Admin", "CampaignManager", "Donor"];
        UserRole[] roles = Enum.GetValues<UserRole>();
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
        }

        //* Adding a Dummy Admin Account
        const string adminEmail = "admin@donationhub.com";
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var sharedId = Guid.NewGuid();
            var domainUser = User.Create(sharedId, "System Admin", "en", DateTime.UtcNow);

            var adminUser = new ApplicationUser
            {
                Id = sharedId,
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                DomainUser = domainUser
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@12345");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, UserRole.Admin.ToString());
        }
    }
}
