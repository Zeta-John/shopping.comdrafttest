using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Models;
using Newtonsoft.Json;

namespace shoppingcomdraft5.Pages.Listings
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;

        public DeleteModel(shoppingcomdraft5.Data.shoppingcomdraft5Context context)
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

            var listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingID == id);

            if (listing == null)
            {
                return NotFound();
            }
            else 
            {
                Listing = listing;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Listing == null)
            {
                return NotFound();
            }
            var listing = await _context.Listing.FindAsync(id);

            if (listing != null)
            {
                Listing = listing;
                _context.Listing.Remove(Listing);
            }

            //Creating an audit record
            if (await _context.SaveChangesAsync() > 0)
            {
                // Create an auditrecord object
                var auditlog = new AuditLog();

                //Obtain logged in user's username
                var userID = User.Identity.Name.ToString();
                auditlog.Username = userID;

                //Audit Action Type
                auditlog.ActionType = "Delete";

                //Time when the event occurred
                auditlog.DateTimeStamp = DateTime.Now;

                //Table Name
                auditlog.TableName = "Listing";

                //Table ID
                auditlog.TableID = Listing.ListingID;

                //Before changes
                auditlog.BeforeChange = JsonConvert.SerializeObject(Listing);

                //After changes
                auditlog.AfterChange = " ";

                _context.AuditLogs.Add(auditlog);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
