using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using control.Data;
using control.Models;
using control.Helper;

namespace control.Pages.Managment.Door
{
    public class RegisterModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public RegisterModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin);

            return Page();
        }

        [BindProperty]
        public control.Models.Door Door { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin);

            _context.Door.Add(Door);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
