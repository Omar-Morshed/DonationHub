using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegistrationDTO userInfo)
    {
        var user = await _authService.RegisterAsync(request: userInfo);
        if (user is null)
            return BadRequest("Error Happened");
        return Ok(user);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> LoginAsync(LoginDTO request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }
}
