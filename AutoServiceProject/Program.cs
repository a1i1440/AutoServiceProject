using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoServiceProject.Models;
using AutoServiceProject.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using AutoServiceProject.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ConfirmEmail");
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" && !context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }
    await next();
});



app.MapRazorPages();
app.MapHub<AutoServiceProject.Hubs.ServiceRequestHub>("/servicerequesthub");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string[] roles = { "Admin", "User", "Mechanic" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }


    var adminEmail = "alimustafaev17@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = "Admin",
            IsPremium = true,
            Address = "Admin Address"
        };

        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }


    var mechanicEmail = "mechanic@service.com";
    var mechanicUser = await userManager.FindByEmailAsync(mechanicEmail);
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (mechanicUser == null)
    {
        mechanicUser = new AppUser
        {
            UserName = mechanicEmail,
            Email = mechanicEmail,
            EmailConfirmed = true,
            FullName = "Salim Islamov",
            IsPremium = false,
            Address = "Garage"
        };

        await userManager.CreateAsync(mechanicUser, "Mechanic123!");
        await userManager.AddToRoleAsync(mechanicUser, "Mechanic");
    }

    bool mechanicExists = dbContext.Mechanics.Any(m => m.FullName == "Salim Islamov");
    if (!mechanicExists)
    {
        var mechanicEntity = new Mechanic
        {
            FullName = mechanicUser.FullName,
            Age = 29,
            SpecializationBrand = "Toyota"
        };

        dbContext.Mechanics.Add(mechanicEntity);
        await dbContext.SaveChangesAsync();
    }


    var userEmail = "alimustafaev594@gmail.com";
    var normalUser = await userManager.FindByEmailAsync(userEmail);
    if (normalUser == null)
    {
        normalUser = new AppUser
        {
            UserName = userEmail,
            Email = userEmail,
            EmailConfirmed = true,
            FullName = "Ali Mustafaev",
            IsPremium = false,
            Address = "User Address"
        };

        await userManager.CreateAsync(normalUser, "Ali123!");
        await userManager.AddToRoleAsync(normalUser, "User");
    }
}



app.Run();
