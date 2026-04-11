using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Identity;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
    {
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = {"Staff", "Customer"};

        foreach(var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role)) 
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var userManager = service.GetRequiredService<UserManager <ApplicationUser>>();
        string adminEmail = "admin@mellon.gr";
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName="admin",
                Email=adminEmail,
                Name="System",
                LastName = "Administrator",
                AFM="000000000",
                EmailConfirmed = true,
                Address = ""
            };

            var result = await userManager.CreateAsync(newAdmin, "password");
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                {
                    Console.WriteLine($"[ERRORXYZ]: {e.Description}");
                }
            }
            await userManager.AddToRoleAsync(newAdmin, "Staff");
        }
    }
}
