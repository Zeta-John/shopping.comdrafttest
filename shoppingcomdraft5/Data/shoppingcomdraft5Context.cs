using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shoppingcomdraft5.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace shoppingcomdraft5.Data
{
    public class shoppingcomdraft5Context : IdentityDbContext<ApplicationUser>
    {
        public shoppingcomdraft5Context (DbContextOptions<shoppingcomdraft5Context> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<shoppingcomdraft5.Models.Listing> Listing { get; set; } = default!;

        public DbSet<shoppingcomdraft5.Models.AuditLog> AuditLogs { get; set; }
    }
}
