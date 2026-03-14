using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Razor_Pages_Testing.Authorization;
using Razor_Pages_Testing.Data;
using Razor_Pages_Testing.Models.Users;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Явно указываем фреймворку, где лежат наши самописные страницы авторизации
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Users/Index", ""); // Это мне было лень делать главную страницу, поэтому я сделал главной другую страницу программно

    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AuthorizeFolder("/Users", "RequiresAdminRole");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Register");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditUserTaskPolicy", policy => policy.Requirements.Add(new UserTaskOwnerRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, UserTaskOwnerHandler>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Вставляю начальные значения, было лень писать метод расширения
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при инициализации базы данных.");
    }
}

app.Run();
