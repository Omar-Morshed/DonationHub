using DonationHub.Application.Features.Auth.DTOs;
using MediatR;

namespace DonationHub.Application.Features.Auth.Register;

public record RegisterCommand(RegistrationDTO UserInfo) : IRequest<bool>;

