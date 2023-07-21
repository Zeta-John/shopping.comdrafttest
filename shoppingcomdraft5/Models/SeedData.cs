using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Data;

namespace shoppingcomdraft5.Models
{
    public class SeedData
    {
        public static void SeedDatabase(shoppingcomdraft5Context context)
        {
            if (context == null || context.Listing == null)
            {
                throw new ArgumentNullException("Null RazorPagesListingContext");
            }

            context.Database.Migrate();

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
    }
}
