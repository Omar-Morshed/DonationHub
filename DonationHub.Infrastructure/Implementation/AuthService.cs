using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Options;
using DonationHub.Domain;
using DonationHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static System.Security.Claims.ClaimTypes;

namespace DonationHub.Infrastructure.Implementation;

public class AuthService(UserManager<ApplicationUser> _userManager, IUnitOfWork _uow, IOptions<JwtOptions> _options) : IAuthService
{
    private readonly JwtOptions jwtOptions = _options.Value;
    public async Task<AuthResponseDTO?> LoginAsync(LoginDTO request)
    {
        var applicationUser = await _userManager.FindByNameAsync(request.Username);
        if (applicationUser is null)
            return null;

        var correctPassword = await _userManager.CheckPasswordAsync(applicationUser, request.Password);

        if (!correctPassword)
            return null;

        //! Unfortunate this is not a transaction, we need to work on it
        var user = await _uow.UserRepository.GetByIdAsync(applicationUser.Id);

        return new AuthResponseDTO(
            AccessToken: await CreateTokenAsync(applicationUser),
            ExpiresAt: DateTime.UtcNow.AddMinutes(15),
            RefreshToken: "", //! need to be implemented
            UserId: applicationUser.Id,
            Email: applicationUser.Email ?? "",
            FullName: applicationUser.UserName ?? "",
            PreferredLanguage: user.PreferredLanguage ?? "en",
            Roles: [.. await _userManager.GetRolesAsync(applicationUser)]
        );
    }

    public async Task<User?> RegisterAsync(RegistrationDTO request)
    {
        var sharedId = Guid.NewGuid();

        var user = User.Create(
            sharedId,
            request.FullName,
            request.PreferredLanguage,
            DateTime.UtcNow
        );

        var applicationUser = new ApplicationUser()
        {
            Id = sharedId,
            UserName = request.Email,
            Email = request.Email,
        };

        //! Need a Data Seeder for putting the required Roles
        // await _userManager.AddToRoleAsync(applicationUser, "Customer");
        var identityResult = await _userManager.CreateAsync(applicationUser, request.Password);

        if (!identityResult.Succeeded)
            return null; //! needs another logic

        _uow.UserRepository.Update(user);
        await _uow.CompleteAsync();
        return user;
    }

    private async Task<string> CreateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

}
