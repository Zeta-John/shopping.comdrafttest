using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Models;

namespace shoppingcomdraft5.Pages.Listings
{
    public class IndexModel : PageModel
    {
        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;

        public IndexModel(shoppingcomdraft5.Data.shoppingcomdraft5Context context)
        {
            _context = context;
        }

        public IList<Listing> Listing { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public SelectList? Category { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? ListingCategory { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> genreQuery = from l in _context.Listing
                                            orderby l.Category
                                            select l.Category;

            var listings = from l in _context.Listing
                         select l;

            if (!string.IsNullOrEmpty(SearchString))
            {
                listings = listings.Where(s => s.ListingName.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(ListingCategory))
            {
                listings = listings.Where(x => x.Category == ListingCategory);
            }
            Category = new SelectList(await genreQuery.Distinct().ToListAsync());
            Listing = await listings.ToListAsync();
        }
    }
}
