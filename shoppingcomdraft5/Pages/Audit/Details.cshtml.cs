using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Models;

namespace shoppingcomdraft5.Pages.Audit
{
    public class DetailsModel : PageModel
    {
        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;

        public DetailsModel(shoppingcomdraft5.Data.shoppingcomdraft5Context context)
        {
            _context = context;
        }

      public AuditLog AuditLog { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.AuditLogs == null)
            {
                return NotFound();
            }

            var auditlog = await _context.AuditLogs.FirstOrDefaultAsync(m => m.Audit_ID == id);
            if (auditlog == null)
            {
                return NotFound();
            }
            else 
            {
                AuditLog = auditlog;
            }
            return Page();
        }
    }
}
