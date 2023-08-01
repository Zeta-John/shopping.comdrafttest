using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Models;

namespace shoppingcomdraft5.Pages.Listings
{
    public class CreateModel : PageModel
    {
        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;

        public CreateModel(shoppingcomdraft5.Data.shoppingcomdraft5Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Listing Listing { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Listing == null || Listing == null)
            {
                return Page();
            }

            _context.Listing.Add(Listing);

            //Creating an audit record
            if (await _context.SaveChangesAsync() > 0)
            {
                // Create an auditrecord object
                var auditlog = new AuditLog();

                //Obtain logged in user's username
                var userID = User.Identity.Name.ToString();
                auditlog.Username = userID;

                //Audit Action Type
                auditlog.ActionType = "Create";

                //Time when the event occurred
                auditlog.DateTimeStamp = DateTime.Now;

                //Table Name
                auditlog.TableName = "Listing";

                //Table ID
                auditlog.TableID = Listing.ListingID;

                //Before changes
                auditlog.BeforeChange = " ";

                //After changes
                auditlog.AfterChange = JsonConvert.SerializeObject(Listing);

                _context.AuditLogs.Add(auditlog);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
