using Casko.AspNetCore.RobotsTxt.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Add robots.txt
builder.Services.AddRobotsTxt();

var app = builder.Build();

app.UseRouting();

app.MapControllerRoute(
    "default",
    "{controller=Default}/{action=Index}/{id?}");

// Use robots.txt
app.UseRobotsTxt(true);

app.Run();