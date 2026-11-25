using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Domain;

namespace DonationHub.Application.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegistrationDTO request, CancellationToken cancellationToken);
    Task<AuthResponseDTO?> LoginAsync(LoginDTO request, CancellationToken cancellationToken);
    Task<bool> LogoutAsync(string rawToken, CancellationToken cancellationToken);
    Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task<UserProfileDTO?> GetCurrentUserAsync(Guid userId);
    // Task<string> GenerateAndSaveRefreshToken(Guid userId);
}
