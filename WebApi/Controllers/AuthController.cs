using Domain.DTOs.Auth;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDTO register)
    {
        var result = await service.RegisterAsync(register);
        if (!result.Succeeded)
        {
            return BadRequest();
        }
        return Ok("Successfully registrated");
    }
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDTO login)
    {
        var token = await service.LoginAsync(login);
        if (token == null)
        {
            return Unauthorized();
        }
        return Ok(new { token });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync(PasswordChangeDTO dTO)
    {
        var resul = await service.ChangePasswordAsync(dTO);
        if (!resul)
        {
            return BadRequest("Email not found or something went wrong");
        }
        return Ok("Password changed successfully");
    }


    [HttpPost("request-password-reset")]
    public async Task<IActionResult> ResedPasswordRequestAsync(string email)
    {
        var result = await service.RequestPasswordResetAsync(email);
        if (!result)
        {
            return BadRequest("User with this email doesn't exist");
        }
        return Ok("Token for reseting password sended!");
    }

    [HttpPost("password-reset")]
    public async Task<IActionResult> ResetPasswordAsync(ResetDTO reset)
    {
        var result = await service.ResetAsync(reset);
        if (!result)
        {
            return BadRequest("Incorect token or email");
        }
        return Ok("Password reseted!");
    }
}
