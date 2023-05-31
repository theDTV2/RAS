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
using System.Drawing.Printing;
using control.Generator;

namespace control.Pages.Managment.User
{
    public class UnlockModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public UnlockModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public new Models.User User { get; set; } = default!;

        [BindProperty]
        public string FirstName { get; set; } = default!;
        [BindProperty]
        public string LastName { get; set; } = default!;
        [BindProperty]
        public string SecretCode { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var _user =  await _context.User.FirstOrDefaultAsync(m => m.UserName == id);
            if (_user == null)
            {
                return NotFound();
            }

            if (!_user.CompletedRegistration)
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "User has not completed registration!");
                return new RedirectResult("List");
            }

            FirstName = _user.FirstName;
            LastName = _user.LastName;
            SecretCode = _user.SecretCode;


            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //TODO: Guard against false input
            //TODO: Add alerts for all possible problems
            User.LastName = LastName;
            User.FirstName = FirstName;
    
            User.SecretCode = SecretCode;

            _context.Attach(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.UserName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./List");
        }

        private bool UserExists(string id)
        {
          return (_context.User?.Any(e => e.UserName == id)).GetValueOrDefault();
        }
    }
}
