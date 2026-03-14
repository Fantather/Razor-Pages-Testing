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

            // Получаем пользователей из базы данных для извлечения их идентификаторов
            var admin = await userManager.FindByEmailAsync("admin@test.com");
            var user = await userManager.FindByEmailAsync("user1@test.com");

            if (admin != null && user != null)
            {
                // Создание задач с привязкой к конкретному пользователю
                for (int i = 1; i <= 5; i++)
                {
                    string taskTitle = $"Task number {i}";

                    if (await dbContext.UserTasks.FirstOrDefaultAsync(t => t.Title == taskTitle) == null)
                    {
                        var newTask = new UserTask
                        {
                            Title = taskTitle,
                            Description = taskTitle,
                            IsDone = false,
                            // Первые 3 задачи отдаем админу, остальные 2 отдаем обычному пользователю
                            OwnerId = i <= 3 ? admin.Id : user.Id
                        };

                        dbContext.UserTasks.Add(newTask);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}