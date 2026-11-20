using DonationHub.Application.Features.Auth.DTOs;
using MediatR;

namespace DonationHub.Application.Features.Auth.Login;

public record LoginCommand(LoginDTO UserInfo) : IRequest<AuthResponseDTO?>;
