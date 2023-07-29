using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Models;

namespace shoppingcomdraft5.Pages.Audit
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
        public AuditLog AuditLog { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.AuditLogs == null || AuditLog == null)
            {
                return Page();
            }

            _context.AuditLogs.Add(AuditLog);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
