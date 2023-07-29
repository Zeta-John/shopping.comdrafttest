using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using shoppingcomdraft5.Models;

namespace shoppingcomdraft5.Pages.Roles
{
    //[Authorize(Roles = "Admin/Owner")]
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;
        public DeleteModel(RoleManager<ApplicationRole> roleManager, Data.shoppingcomdraft5Context context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationRole = await _roleManager.FindByIdAsync(id);
            if (ApplicationRole == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            ApplicationRole = await _roleManager.FindByIdAsync(id);

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
            auditlog.TableName = "Roles";

            //Table ID
            auditlog.TableID = -1;

            //Before changes
            auditlog.BeforeChange = JsonConvert.SerializeObject(ApplicationRole);

            //After changes
            auditlog.AfterChange = " ";

            _context.AuditLogs.Add(auditlog);
            await _context.SaveChangesAsync();

            IdentityResult roleRuslt = await _roleManager.DeleteAsync(ApplicationRole);
            return RedirectToPage("./Index");
        }
    }
}