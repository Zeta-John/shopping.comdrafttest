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
    public class IndexModel : PageModel
    {
        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;

        public IndexModel(shoppingcomdraft5.Data.shoppingcomdraft5Context context)
        {
            _context = context;
        }

        public IList<AuditLog> AuditLog { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.AuditLogs != null)
            {
                AuditLog = await _context.AuditLogs.ToListAsync();
            }
        }
    }
}
