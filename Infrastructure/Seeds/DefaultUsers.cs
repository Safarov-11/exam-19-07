using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeds;

public class DefaultUsers
{
    public static async Task SeedUserAsync(UserManager<IdentityUser> userManager)
    {
        var existing = await userManager.FindByNameAsync("Muhammad");
        if (existing != null)
        {
            return;
        }

        var user = new IdentityUser()
        {
            UserName = "Muhammad",
            Email = "romasafarov.ru@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "000080864",
            PhoneNumberConfirmed = true
        };

        await userManager.CreateAsync(user, "1111");
        await userManager.AddToRoleAsync(user, "Admin");
    }
}
