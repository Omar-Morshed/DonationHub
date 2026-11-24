using MediatR;

namespace DonationHub.Application.Features.Auth.Logout;

public record LogoutCommand(string RawToken) : IRequest<bool>;
