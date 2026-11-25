using DonationHub.Application.Interfaces;
using MediatR;

namespace DonationHub.Application.Features.Admin.CreateStaff;

public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, bool>
{
    private readonly IAuthService _authService;

    public CreateStaffCommandHandler(IAuthService authService)
    {
        this._authService = authService;

    }
    public async Task<bool> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.CreateStaffAsync(request.Request);
        return result;
    }

}
