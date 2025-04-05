using EvaluationNexaQuanta.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EvaluationNexaQuanta.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add database context and repositories
builder.Services.AddSingleton<DapperDbContext>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<ProductRepository>();

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";  // Specify login path
    });

// Add Authorization and MVC
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();  // Add MVC controller support

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default routing for MVC (going to UsersController -> Login)l
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}/{id?}"); // Default route

await UsersRepository.SeedAdminUser(app);
app.Run();
