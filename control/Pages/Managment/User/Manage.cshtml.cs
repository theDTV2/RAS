using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using control.Data;
using control.Models;
using control.Helper;

namespace control.Pages.Managment.User
{
    public class ManageModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ManageModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public control.Models.User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.User == null)
            return NotFound();

            var user =  await _context.User.FirstOrDefaultAsync(m => m.UserName == id);
            if (user == null)
            return NotFound();

            var _userEAccessLevel = AccountHelper.GetEAccessLevel(HttpContext);
            if (user.AccessLevel >= _userEAccessLevel && _userEAccessLevel !=EAccessLevel.kSuperAdmin )
                return NotFound();

            User = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var _userEAccessLevel = AccountHelper.GetEAccessLevel(HttpContext);
            if (User.AccessLevel >= _userEAccessLevel && _userEAccessLevel != EAccessLevel.kSuperAdmin)
                return NotFound();

            if (User.AccessLevel == EAccessLevel.kModerator)
                return NotFound();
            //TODO: Log Manipulation attempt

            if (!ModelState.IsValid)
                return Page();

              //We use TryUpdateModelAsync to prevent data manipulation
            if (await TryUpdateModelAsync<control.Models.User>(
                User,
                "User",
                u => u.FirstName, u => u.LastName, u => u.AccessLevel,
                u => u.ExpiryDate, u => u.SecretCode
                ))
            try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) 
                {
                    return NotFound();
                }
            return RedirectToPage("./List");
        }

    }
}
