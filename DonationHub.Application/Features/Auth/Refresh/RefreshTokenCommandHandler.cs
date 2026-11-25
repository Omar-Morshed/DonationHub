using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Application.Interfaces;
using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDTO?>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        this._authService = authService;
    }

    public async Task<AuthResponseDTO?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (request.RefreshToken is null)
            return null;
        var newRawToken = await _authService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
        return newRawToken;
    }

}
