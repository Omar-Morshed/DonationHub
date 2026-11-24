using DonationHub.Application.Interfaces;
using MediatR;

namespace DonationHub.Application.Features.Auth.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LogoutAsync(request.RawToken, cancellationToken);
        return result;
    }

}
