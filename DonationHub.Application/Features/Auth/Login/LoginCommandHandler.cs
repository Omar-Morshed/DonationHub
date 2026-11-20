using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Application.Interfaces;
using MediatR;

namespace DonationHub.Application.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDTO?>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDTO?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.UserInfo);
        return result;
    }

}
