using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh;

public record RefreshTokenCommand(Guid UserId) : IRequest<string?>;
