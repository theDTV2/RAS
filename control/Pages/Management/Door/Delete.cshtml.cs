using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using control.Data;
using control.Models;
using control.Helper;

namespace control.Pages.Management.Door
{
    public class DeleteModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public DeleteModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public control.Models.Door Door { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string DoorIdentifier { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            if (DoorIdentifier == null || _context.Door == null)
            {
                return NotFound();
            }

            var door = await _context.Door.FirstOrDefaultAsync(m => m.Id == DoorIdentifier);

            if (door == null)
            {
                return NotFound();
            }
            else
            {
                Door = door;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            if (DoorIdentifier == null || _context.Door == null)
            {
                return NotFound();
            }
            var door = await _context.Door.FindAsync(DoorIdentifier);

            if (door != null)
            {
                Door = door;
                _context.Door.Remove(Door);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./List");
        }
    }
}
