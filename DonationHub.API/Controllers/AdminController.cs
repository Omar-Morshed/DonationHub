using DonationHub.Application.Features.Admin.CreateStaff;
using DonationHub.Application.Features.Admin.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ISender _sender;

    public AdminController(ISender sender)
    {
        this._sender = sender;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Staff")]
    public async Task<IActionResult> StaffAsync(CreateStaffRequestDTO requestDTO)
    {
        var result = await _sender.Send(new CreateStaffCommand(requestDTO));
        if (!result)
            return BadRequest();
        return Ok();
    }
}
