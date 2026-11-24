using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Options;
using DonationHub.Domain;
using DonationHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using static System.Security.Claims.ClaimTypes;

namespace DonationHub.Infrastructure.Implementation;

public class AuthService(UserManager<ApplicationUser> _userManager, IUnitOfWork _uow, IOptions<JwtOptions> _options) : IAuthService
{
    private readonly JwtOptions jwtOptions = _options.Value;
    public async Task<AuthResponseDTO?> LoginAsync(LoginDTO request, CancellationToken cancellationToken)
    {
        var applicationUser = await _userManager.Users
            .Include(appUser => appUser.DomainUser)
            .FirstOrDefaultAsync(appUser => appUser.Email == request.Username, cancellationToken);

        if (applicationUser is null)
            return null;

        var correctPassword = await _userManager.CheckPasswordAsync(applicationUser, request.Password);

        if (!correctPassword)
            return null;

        //! Unfortunate this is not a transaction, we need to work on it

        //! I don't like that we make 2 Queries to get the User, One using the ApplicationUser and the other with the normal user
        //* Solved by making a Navigation Property in the Application User

        // // var user = await _uow.UserRepository.GetByIdAsync(applicationUser.Id);

        return new AuthResponseDTO(
            AccessToken: await CreateTokenAsync(applicationUser),
            ExpiresAt: DateTime.UtcNow.AddMinutes(15),
            RefreshToken: await GenerateAndSaveRefreshToken(applicationUser.DomainUser.Id),
            UserId: applicationUser.Id,
            Email: applicationUser.Email ?? "",
            FullName: applicationUser.DomainUser.FullName ?? "",
            PreferredLanguage: applicationUser.DomainUser.PreferredLanguage ?? "en",
            Roles: [.. await _userManager.GetRolesAsync(applicationUser)]
        );
    }

    public async Task<User?> RegisterAsync(RegistrationDTO request, CancellationToken cancellationToken)
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
            DomainUser = user
        };

        var identityResult = await _userManager.CreateAsync(applicationUser, request.Password);
        await _userManager.AddToRoleAsync(applicationUser, "Donor");

        if (!identityResult.Succeeded)
            return null; //! needs another logic

        return user;
    }

    public async Task<bool> LogoutAsync(string rawToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
            return false;

        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

        var storedToken = await _uow.RefreshTokenRepository.GetByTokenHash(tokenHash);

        if (storedToken is null)
            return false;

        storedToken.Revoke(DateTime.UtcNow);
        await _uow.CompleteAsync(cancellationToken);
        return true;
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

    public async Task<string> GenerateAndSaveRefreshToken(Guid userId)
    {
        var rawToken = GenerateRefreshToken();
        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));
        var refreshToken = RefreshToken.Create(
            userId: userId,
            tokenHash: tokenHash,
            createAt: DateTime.UtcNow,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _uow.RefreshTokenRepository.AddAsync(refreshToken);
        await _uow.CompleteAsync();

        return rawToken;
    }
    private static string GenerateRefreshToken()
    {
        // 1. حجز مصفوفة في الذاكرة حجمها 32 بايت (32 * 8 = 256 بت)
        var randomNumber = new byte[32];

        // 2. إنشاء مولد أرقام عشوائية مشفر وتدميره تلقائياً بعد الانتهاء (using)
        using var rng = RandomNumberGenerator.Create();

        // 3. ملء الـ 32 بايت بأرقام عشوائية حقيقية من نظام التشغيل
        rng.GetBytes(randomNumber);

        // 4. تحويل البايتات الخام لنص Base64 مقروء ينفع يتبعت في JSON
        return Convert.ToBase64String(randomNumber);
    }
}
