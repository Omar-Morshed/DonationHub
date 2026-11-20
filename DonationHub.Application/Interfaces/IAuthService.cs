using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Domain;

namespace DonationHub.Application.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegistrationDTO request);
    Task<AuthResponseDTO?> LoginAsync(LoginDTO request);
}
