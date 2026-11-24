using DonationHub.Application.Interfaces;
using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string?>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        this._authService = authService;
    }

    public async Task<string?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var newRawToken = await _authService.GenerateAndSaveRefreshToken(request.UserId);
        return newRawToken;
    }

}
