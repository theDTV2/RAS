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

namespace control.Pages.Managment.Door
{
    public class ManageModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ManageModel(control.Data.controlContext context)
        {
            _context = context;
        }

      public control.Models.Door Door { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin);
            if (id == null || _context.Door == null)
            {
                return NotFound();
            }

            var door = await _context.Door.FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
