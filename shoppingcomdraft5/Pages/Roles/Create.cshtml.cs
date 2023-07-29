using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using shoppingcomdraft5.Models;
using System;
using System.Data;
using System.Threading.Tasks;
namespace shoppingcomdraft5.Pages.Roles
{
    //[Authorize(Roles = "Admin/Owner")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;
        public CreateModel(RoleManager<ApplicationRole> roleManager, shoppingcomdraft5.Data.shoppingcomdraft5Context context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationRole.CreatedDate = DateTime.UtcNow;
            ApplicationRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
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
            auditlog.TableName = "Roles";

            //Table ID
            auditlog.TableID = -1;

            //Before changes
            auditlog.BeforeChange = " ";

            //After changes
            auditlog.AfterChange = JsonConvert.SerializeObject(ApplicationRole);

            _context.AuditLogs.Add(auditlog);
            await _context.SaveChangesAsync();
            IdentityResult roleRuslt = await _roleManager.CreateAsync(ApplicationRole);
            return RedirectToPage("Index");
        }
    }
}
