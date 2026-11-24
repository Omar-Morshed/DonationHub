using DonationHub.Application.Interfaces;
using MediatR;

namespace DonationHub.Application.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.RegisterAsync(request: request.UserInfo, cancellationToken);
        return user is not null;
    }

}
