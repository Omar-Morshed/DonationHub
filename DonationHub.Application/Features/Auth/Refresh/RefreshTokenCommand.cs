using DonationHub.Application.Features.Auth.DTOs;
using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh;

public record RefreshTokenCommand(string? RefreshToken) : IRequest<AuthResponseDTO?>;
