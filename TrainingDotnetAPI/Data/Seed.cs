using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingDotnetAPI.Models;

namespace TrainingDotnetAPI.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole {Name = "Admin"},
                    new IdentityRole {Name = "Member"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!await userManager.Users.AnyAsync())
            {
                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(adminUser, "123456");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}