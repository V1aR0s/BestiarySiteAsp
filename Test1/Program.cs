using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Test1;
using Test1.CustomPolicy;
using Test1.Models;

//User client id
//62696112088-sipmgm739j6bb7ddalcb7v2cmesi8ri9.apps.googleusercontent.com

//UserClientSecret
//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
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DeaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllersWithViews();

//Подключаем аутентификацию через гугл
builder.Services.AddAuthentication().AddGoogle(options =>
{
    //IConfiguration googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = NotToGit.GoogleClientId;
    options.ClientSecret = NotToGit.GoogleClientSecret;

});
/*
builder.Services.AddAuthentication().AddFacebook(options =>
{
    IConfiguration googleAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
    options.ClientId = googleAuthNSection["AppId"];
    options.ClientSecret = googleAuthNSection["AppSecret"];

});
builder.Services.AddAuthentication().AddTwitter(options =>
{
    IConfiguration googleAuthNSection = builder.Configuration.GetSection("Authentication:Twitter");
    options.ConsumerKey = googleAuthNSection["ClientId"];
    options.ConsumerSecret = googleAuthNSection["ClientSecret"];

});
*/

//builder.Services.AddSingleton<IAuthorizationHandler, IsAllDataHandler>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAcces", policy => { policy.RequireRole("Admin"); });
    options.AddPolicy("IsAllData", policy => { policy.AddRequirements(new IsAllDataPolicy()); });
    //options.AddPolicy("IsAllDataSec", policy => { policy.AddRequirements(new AllDataCheckSecond()); });
});

builder.Services.AddTransient<IAuthorizationHandler, IsAllDataHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await InitializeAdmin.Initialization(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


