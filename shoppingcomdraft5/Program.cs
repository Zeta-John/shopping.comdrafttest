using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Data;
using Microsoft.Extensions.DependencyInjection;
using shoppingcomdraft5.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SendGrid;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<shoppingcomdraft5Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("shoppingcomdraft5Context") ?? throw new InvalidOperationException("Connection string 'shoppingcomdraft5Context' not found.")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultUI()
    .AddEntityFrameworkStores<shoppingcomdraft5Context>()
    .AddDefaultTokenProviders();

builder.Services.AddMvc()
    .AddRazorPagesOptions(options =>
    {
        // options.Conventions.AllowAnonymousToFolder("/Listings");
        // options.Conventions.AuthorizePage("/Listings/Create");
        // options.Conventions.AuthorizeAreaPage("Identity", "/Manage/Accounts");
        // options.Conventions.AuthorizeFolder("/Listings");
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
    // User settings
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientID"];
        options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
var sendGridApiKey = builder.Configuration["SendGrid:ApiKey"];
builder.Services.AddSingleton<ISendGridClient>(new SendGridClient(sendGridApiKey));
var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<shoppingcomdraft5Context>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var seedData = new SeedData(userManager, roleManager);
    seedData.SeedDatabase(context);
}



app.Run();