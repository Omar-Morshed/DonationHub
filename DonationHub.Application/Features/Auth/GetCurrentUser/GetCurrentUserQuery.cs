using DonationHub.Application.Features.Auth.DTOs;
using MediatR;

namespace DonationHub.Application.Features.Auth.GetCurrentUser;

public record GetCurrentUserQuery(Guid UserId) : IRequest<UserProfileDTO?>;
