using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Auth;

public class PasswordChangeDTO
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    
    [Compare("NewPassword")]
    public string ConfrmPassword { get; set; } = null!;
}
