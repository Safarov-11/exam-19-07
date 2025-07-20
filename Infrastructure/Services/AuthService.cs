using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.Auth;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    IConfiguration config,
    IHttpContextAccessor contextAccessor,
    IEmailService emailService
) : IAuthService
{
    public async Task<IdentityResult> RegisterAsync(RegisterDTO model)
    {
        var user = new IdentityUser()
        {
            UserName = model.Email,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");

        }
        return result;
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.UserName!),
            new (ClaimTypes.Email, user.Email!),
            new (ClaimTypes.MobilePhone, user.PhoneNumber!),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var secretKey = config["Jwt:Key"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string?> LoginAsync(LoginDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return null;
        }

        var result = await userManager.CheckPasswordAsync(user, model.Password);
        return !result
        ? null
        : await GenerateJwtToken(user);
    }

    public async Task<bool> ChangePasswordAsync(PasswordChangeDTO model)
    {
        var userEmail = contextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await userManager.FindByEmailAsync(userEmail!);
        if (user == null)
        {
            return false;
        }

        var changePassword = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        return changePassword.Succeeded;
    }

    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        await emailService.SendResetPasswordTokenToEmailAsync(email, token);
        return true;
    }

    public async Task<bool> ResetAsync(ResetDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return false;
        }

        var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        return true;    

    }
}
