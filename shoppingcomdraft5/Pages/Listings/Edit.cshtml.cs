using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Models;

namespace shoppingcomdraft5.Pages.Listings
{
    public class EditModel : PageModel
    {
        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;

        public EditModel(shoppingcomdraft5.Data.shoppingcomdraft5Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Listing Listing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Listing == null)
            {
                return NotFound();
            }

            var listing =  await _context.Listing.FirstOrDefaultAsync(m => m.ListingID == id);
            if (listing == null)
            {
                return NotFound();
            }
            Listing = listing;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Creating an audit record

            var auditlog = new AuditLog();

            //Obtain logged in user's username
            var userID = User.Identity.Name.ToString();
            auditlog.Username = userID;

            //Audit Action Type
            auditlog.ActionType = "Edit";

            //Time when the event occurred
            auditlog.DateTimeStamp = DateTime.Now;

            //Table Name
            auditlog.TableName = "Listing";

            //Table ID
            auditlog.TableID = Listing.ListingID;

            //Before changes
            var listingBeforeChanges = await _context.Listing.AsNoTracking().FirstOrDefaultAsync(m => m.ListingID == Listing.ListingID);
            auditlog.BeforeChange = JsonConvert.SerializeObject(listingBeforeChanges);

            //After changes
            auditlog.AfterChange = JsonConvert.SerializeObject(Listing);
        

            _context.Attach(Listing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                _context.AuditLogs.Add(auditlog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListingExists((int)Listing.ListingID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ListingExists(int id)
        {
          return (_context.Listing?.Any(e => e.ListingID == id)).GetValueOrDefault();
        }
    }
}
