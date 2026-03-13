using Microsoft.AspNetCore.Identity;
using Razor_Pages_Testing.Models.Users;
using Razor_Pages_Testing.Models.UserTasks;
using Microsoft.EntityFrameworkCore;

namespace Razor_Pages_Testing.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Создание ролей
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Создание пользователя-администратора
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

            // Создание базового пользователя для входа
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

            // Создание дополнительных пользователей для массовки в таблице
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

            // Создание задач
            for (int i = 1; i <= 5; i++)
            {
                string taskTitle = $"Task number {i}";

                if (await dbContext.UserTasks.FirstOrDefaultAsync(t => t.Title == taskTitle) == null)
                {
                    var newTask = new UserTask
                    {
                        Title = taskTitle,
                        Description = taskTitle,
                        IsDone = false
                    };

                    dbContext.UserTasks.Add(newTask);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}