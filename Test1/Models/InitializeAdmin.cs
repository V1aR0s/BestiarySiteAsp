using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Test1.Models
{
    public static class InitializeAdmin
    {

        public static async Task Initialization(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string LoginAdmin = "NotTryHard";
            string adminMail = @"vlaros19992004@gmail.com";
            string password = "Admin010203_";

            if(await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (await roleManager.FindByNameAsync("Creator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Creator"));
            }

            if (await userManager.FindByEmailAsync(adminMail) == null)
            {
                User admin = new User { UserName = LoginAdmin, Email = adminMail, Year = 2004 };
                IdentityResult result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    User UserClaimSuer = await userManager.FindByEmailAsync(admin.Email);
                    Claim claim = new Claim("IsAllData", "Yes", ClaimValueTypes.String);
                    IdentityResult resultClaim = await userManager.AddClaimAsync(UserClaimSuer, claim);
                    if (resultClaim.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }

    }
}
