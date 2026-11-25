using DonationHub.Application.Features.Admin.DTOs;
using MediatR;

namespace DonationHub.Application.Features.Admin.CreateStaff;

public record CreateStaffCommand(CreateStaffRequestDTO Request) : IRequest<bool>;
