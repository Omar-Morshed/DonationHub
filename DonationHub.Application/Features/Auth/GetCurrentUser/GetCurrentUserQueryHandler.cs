using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Application.Interfaces;
using MediatR;

namespace DonationHub.Application.Features.Auth.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserProfileDTO?>
{
    private readonly IAuthService _authService;

    public GetCurrentUserQueryHandler(IAuthService authService)
    {
        this._authService = authService;

    }
    public async Task<UserProfileDTO?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetCurrentUserAsync(request.UserId);
        return result;
    }

}
