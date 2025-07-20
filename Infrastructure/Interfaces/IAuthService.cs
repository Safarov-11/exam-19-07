using Domain.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IAuthService
{
    public Task<IdentityResult> RegisterAsync(RegisterDTO model);
    public Task<string?> LoginAsync(LoginDTO model);
    public Task<bool> ChangePasswordAsync(PasswordChangeDTO model);
    public Task<bool> RequestPasswordResetAsync(string email);
    public Task<bool> ResetAsync(ResetDTO model);
}

