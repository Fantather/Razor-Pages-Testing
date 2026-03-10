using Microsoft.AspNetCore.Identity;
using Razor_Pages_Testing.Models.Users;

namespace Razor_Pages_Testing.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Создание ролей
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Создание пользователя-администратора
            if (await userManager.FindByEmailAsync("admin@test.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    Age = 30
                };

                var result = await userManager.CreateAsync(adminUser, "Admin_1234!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. Создание базового пользователя для входа
            if (await userManager.FindByEmailAsync("user1@test.com") == null)
            {
                var normalUser = new ApplicationUser
                {
                    UserName = "user1@test.com",
                    Email = "user1@test.com",
                    Age = 25
                };

                var result = await userManager.CreateAsync(normalUser, "User_1234!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }

            // 4. Создание дополнительных пользователей для массовки в таблице
            for (int i = 2; i <= 3; i++)
            {
                string email = $"user{i}@test.com";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var extraUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        Age = 20 + i
                    };

                    var result = await userManager.CreateAsync(extraUser, "Qwerty_123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(extraUser, "User");
                    }
                }
            }
        }
    }
}