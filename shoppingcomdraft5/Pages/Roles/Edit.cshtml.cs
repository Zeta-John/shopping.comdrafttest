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
    public class EditModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly shoppingcomdraft5.Data.shoppingcomdraft5Context _context;
        public EditModel(RoleManager<ApplicationRole> roleManager, Data.shoppingcomdraft5Context context)
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationRole appRole = await _roleManager.FindByIdAsync(ApplicationRole.Id);

            appRole.Id = ApplicationRole.Id;

            // Create an auditlog object
            var auditlog = new AuditLog();

            //Obtain logged in user's username
            var userID = User.Identity.Name.ToString();
            auditlog.Username = userID;

            //Audit Action Type
            auditlog.ActionType = "Edit";

            //Time when the event occurred
            auditlog.DateTimeStamp = DateTime.Now;

            //Table Name
            auditlog.TableName = "Roles";

            //Table ID
            auditlog.TableID = -1;

            //Before changes
            auditlog.BeforeChange = JsonConvert.SerializeObject(appRole); 

            appRole.Name = ApplicationRole.Name;
            appRole.Description = ApplicationRole.Description;

            //After changes
            //Before changes
            auditlog.AfterChange = JsonConvert.SerializeObject(appRole);
            _context.AuditLogs.Add(auditlog);
            await _context.SaveChangesAsync();

            IdentityResult roleRuslt = await _roleManager.UpdateAsync(appRole);
            if (roleRuslt.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            return RedirectToPage("./Index");
        }
    }
}