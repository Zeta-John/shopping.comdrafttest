using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Data;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace shoppingcomdraft5.Models
{
    public class SeedData
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void SeedDatabase(shoppingcomdraft5Context context)
        {
            if (context == null || context.Listing == null)
            {
                throw new ArgumentNullException("Null RazorPagesListingContext");
            }
            context.Database.Migrate();
            SeedRoles();
            SeedUsers();
            // Look for any listings.
            if (context.Listing.Any())
            {
                return;   // DB has been seeded
            }

            context.Listing.AddRange(
                new Listing
                {
                    ListingName = "Iphone14",
                    ListingDate = DateTime.Parse("2023-2-12"),
                    Category = "Electronics",
                    Price = 1200.21M,
                    Condition = "5*"
                },

                new Listing
                {
                    ListingName = "Iphone13",
                    ListingDate = DateTime.Parse("2023-1-22"),
                    Category = "Electronics",
                    Price = 1400.99M,
                    Condition = "5*"
                },

                new Listing
                {
                    ListingName = "IphoneXR",
                    ListingDate = DateTime.Parse("2023-1-1"),
                    Category = "Electronics",
                    Price = 1300.99M,
                    Condition = "5*"
                },

                new Listing
                {
                    ListingName = "Sofa",
                    ListingDate = DateTime.Parse("2022-12-12"),
                    Category = "Furniture",
                    Price = 1000.99M,
                    Condition = "5*"
                }

                
            );
            context.SaveChanges();
        }

        private void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("Owner").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Owner"
                };
                _roleManager.CreateAsync(role).Wait();
            }

            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                _roleManager.CreateAsync(role).Wait();
            }

            if (!_roleManager.RoleExistsAsync("Staff").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Staff"
                };
                _roleManager.CreateAsync(role).Wait();
            }

            if (!_roleManager.RoleExistsAsync("Member").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Member"
                };
                _roleManager.CreateAsync(role).Wait();
            }
        }

        private void SeedUsers()
        {
            if (_userManager.FindByEmailAsync("owner@gmail.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "owner@gmail.com",
                    Email = "owner@gmail.com",
                    // Add any other properties you need
                };

                var result = _userManager.CreateAsync(user, "12345@Ss").Result;
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Owner").Wait();
                }
            }
     
        }
    }
}
