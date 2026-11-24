using DonationHub.Application.Features.Auth.DTOs;
using DonationHub.Application.Features.Auth.Login;
using DonationHub.Application.Features.Auth.Logout;
using DonationHub.Application.Features.Auth.Refresh;
using DonationHub.Application.Features.Auth.Register;
using DonationHub.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ISender _sender;

    public AuthController(IAuthService authService, ISender sender)
    {
        _authService = authService;
        _sender = sender;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegistrationDTO userInfo)
    {
        var result = await _sender.Send(new RegisterCommand(userInfo));
        if (!result)
            return BadRequest();
        return Ok();
        /* var user = await _authService.RegisterAsync(request: userInfo);
        if (user is null)
            return BadRequest("Error Happened");
        return Ok(user); */
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponseDTO>> LoginAsync(LoginDTO request)
    {
        var result = await _sender.Send(new LoginCommand(request));
        if (result is null)
            return BadRequest("Invalid Username or Password !");
        return Ok(result);
        /* var result = await _authService.LoginAsync(request);
        return Ok(result); */
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> LogoutAsync(string rawToken)
    {
        var result = await _sender.Send(new LogoutCommand(rawToken));
        if (!result)
            return BadRequest("Wrong Refresh Token !");
        return Ok("Done !");
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> RefreshTokenAsync(Guid UserId)
    {
        var newRawToken = await _sender.Send(new RefreshTokenCommand(UserId));
        if (newRawToken is null)
            return BadRequest("IDK");
        return Ok(newRawToken);
    }

    [Authorize]
    [HttpGet("Me")]
    public IActionResult Me()
    {
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Admin/Staff")]
    public IActionResult Staff()
    {
        return Ok();
    }
}
