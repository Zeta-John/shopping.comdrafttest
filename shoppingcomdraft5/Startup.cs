using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Models;
using Microsoft.AspNetCore.Authentication.Google;
namespace shoppingcomdraft5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<shoppingcomdraft5Context>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("shoppingcomdraft5Context")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddDefaultUI()
             .AddEntityFrameworkStores<shoppingcomdraft5Context>()
             .AddDefaultTokenProviders();

            /*make sure that need to log in to view listings */
            services.AddMvc()
           .AddRazorPagesOptions(options =>
           {
               // options.Conventions.AllowAnonymousToFolder("/Movies");
               // options.Conventions.AuthorizePage("/Movies/Create");
               // options.Conventions.AuthorizeAreaPage("Identity", "/Manage/Accounts");
               options.Conventions.AuthorizeFolder("/Listings");
           });

            services.Configure<IdentityOptions>(options =>
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
           services.AddAuthentication()
                 .AddGoogle(GoogleOptions =>
                 {
                     GoogleOptions.ClientId = Configuration["Google:ClientID"];
                     GoogleOptions.ClientSecret = Configuration["Google:ClientSecret"];
                 });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
